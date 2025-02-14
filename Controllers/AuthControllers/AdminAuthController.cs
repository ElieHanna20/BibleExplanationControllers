using BibleExplanationControllers.Dtos.AdminDtos;
using BibleExplanationControllers.Dtos.AuthDtos;
using BibleExplanationControllers.Helpers;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.AuthControllers
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AdminAuthController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminSettings loginDto)
        {
            var admin = await _userManager.FindByNameAsync(loginDto.Username);
            if (admin == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // Validate password without creating a session
            bool isValidPassword = await _userManager.CheckPasswordAsync(admin, loginDto.Password);
            if (!isValidPassword)
                return Unauthorized(new { message = "Invalid credentials" });

            // Get user roles
            var roles = await _userManager.GetRolesAsync(admin);

            // Generate tokens
            var (accessToken, refreshToken) = TokenHelper.GenerateJwtToken(admin, _configuration, roles);

            // Save refresh token & expiry in DB
            admin.RefreshToken = refreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(admin);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshRequestDto refreshDto)
        {
            // Filter for Admin users only
            var user = await _userManager.Users
                .OfType<Admin>()
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshDto.RefreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            // Get roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            // Generate new tokens
            var (newAccessToken, newRefreshToken) = TokenHelper.GenerateJwtToken(user, _configuration, roles);

            // Update refresh token and expiry in DB
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
