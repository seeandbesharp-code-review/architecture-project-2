using Microsoft.AspNetCore.Mvc;
using SaleApi.Services;
using static SaleApi.Dto.UserDto;

namespace SaleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserService userService,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            var result = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var expiryMinutes = _configuration.GetValue("JwtSettings:ExpiryMinutes", 60);

            Response.Cookies.Append("access_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = !_environment.IsDevelopment(),
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
            });

            return Ok(result);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserCreateDto createDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createDto);
                return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
