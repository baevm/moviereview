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

            var token = userRepository.GenerateToken(user, loginUserDto);

            if (token.Length == 0)
            {
                return BadRequest("Username or password is incorrect");
            }

            return Ok(token);
        }

        // [Authorize]
        // [HttpPost("refresh")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> Refresh()
        // {
        //     var user = await userRepository.GetUser();

        //     if (user == null)
        //     {
        //         return BadRequest("Username or password is incorrect");
        //     }

        //     var token = userRepository.GenerateToken(user, refreshTokenDto);

        //     if (token.Length == 0)
        //     {
        //         return BadRequest("Username or password is incorrect");
        //     }

        //     return Ok(token);
        // }
    }
}