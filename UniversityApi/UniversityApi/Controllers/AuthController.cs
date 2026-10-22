
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversityApi.DTO.Auth;
using UniversityApi.Identity;
using UniversityApi.Services.Implementations;

namespace UniversityApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Register(
            [FromBody] RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(
                request.Email);

            if (existingUser != null)
            {
                return Conflict(new
                {
                    message = "Пользователь с таким email уже существует."
                });
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(
                user,
                request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(
                        error => error.Description)
                });
            }
            var roleResult = await _userManager.AddToRoleAsync(user, "Student");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return StatusCode(500, new
                {
                    message = "Не удалось назначить роль Student."
                });
            }
            var roles = await _userManager.GetRolesAsync(user);

            var (token, expiresAt) =
                _jwtTokenService.CreateToken(user, roles);

            return Ok(new AuthResponse
            {
                AccessToken = token,
                ExpiresAt = expiresAt,
                Email = user.Email!
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login(
            [FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(
                request.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Неверный email или пароль."
                });
            }

            var result =
                await _signInManager.CheckPasswordSignInAsync(
                    user,
                    request.Password,
                    lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return Unauthorized(new
                {
                    message = "Неверный email или пароль."
                });
            }

            var roles = await _userManager.GetRolesAsync(user);

            var (token, expiresAt) =
                _jwtTokenService.CreateToken(user, roles);

            return Ok(new AuthResponse
            {
                AccessToken = token,
                ExpiresAt = expiresAt,
                Email = user.Email!
            });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var email = User.FindFirstValue(
                ClaimTypes.Email);

            return Ok(new
            {
                message = "JWT-авторизация работает.",
                userId,
                email
            });
        }
    }
}
