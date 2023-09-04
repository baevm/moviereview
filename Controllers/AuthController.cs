using Microsoft.AspNetCore.Mvc;
using moviereview.Dto;
using moviereview.Interfaces;

namespace moviereview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        public AuthController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            var isUserCreated = await userRepository.CreateUser(createUserDto);

            if (!isUserCreated)
            {
                return BadRequest("Username or email already exist");
            }

            return Ok("User created successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
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
    }
}