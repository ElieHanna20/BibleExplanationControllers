
using Microsoft.AspNetCore.Mvc;
using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.AuthDtos;
using BibleExplanationControllers.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.AuthControllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly UserAuthentication _userAuthentication;

        public AuthController(AuthDbContext context, PasswordHasher passwordHasher, UserAuthentication userAuthentication)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userAuthentication = userAuthentication;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
                return Unauthorized(new { message = "Invalid credentials" });

            var role = user.GetType().Name; // "Admin", "SubAdmin", or "Worker"

            // Use UserAuthentication to handle the login
            var success = await _userAuthentication.LoginAsync(user, role, HttpContext);
            if (!success)
                return Unauthorized(new { message = "Login failed" });

            // Store session data (already done by UserAuthentication)
            return Ok(new { message = "Login successful" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Use UserAuthentication to handle the logout
            await _userAuthentication.LogoutAsync(HttpContext);

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
