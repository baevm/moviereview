using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moviereview.Dto;
using moviereview.Interfaces;

namespace moviereview.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        public AuthController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUserCreated = await userRepository.CreateUser(createUserDto);

            if (!isUserCreated)
            {
                return BadRequest("Username or email already exist");
            }

            return Ok("User created successfully");
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userRepository.GetUser(loginUserDto.Username);

            if (user == null)
            {
                return BadRequest("Username or password is incorrect");
            }

            if (!userRepository.VerifyPassword(user.PasswordHash, loginUserDto.Password))
            {
                return Unauthorized();
            }

            try
            {
                var accessToken = userRepository.GenerateAccessToken(user);
                var refreshToken = userRepository.GenerateRefreshToken(user);

                return Ok(new TokensDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception)
            {
                return BadRequest("Username or password is incorrect");
            }
        }

        [Authorize]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isValidToken = userRepository.ValidateToken(Request.Headers["CHANGE_THIS_TO_REFRESH"]);

            if (!isValidToken)
            {
                return Unauthorized();
            }

            var userId = HttpContext.User.FindFirstValue("id");

            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await userRepository.GetUser(userId);

            if (user == null)
            {
                return Unauthorized();
            }

            try
            {
                var accessToken = userRepository.GenerateAccessToken(user);

                return Ok(new TokensDto
                {
                    AccessToken = accessToken
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}