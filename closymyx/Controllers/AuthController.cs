using Microsoft.AspNetCore.Mvc;
using closymyx.DTOs;
using closymyx.Services;

namespace closymyx.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var succes = await _userService.RegisterUserAsync(dto);
            if (!succes)
            {
                return BadRequest("Пользователь с таким email уже существует!");
            }
            else
            {
                return Ok("Регистрация успешна!");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var authResult = await _userService.LoginAsync(dto);
            if (authResult == null)
            {
                return Unauthorized("Неверный email или пароль");
            }
            else
            {
                return Ok(authResult);
            }
        }
    }
}