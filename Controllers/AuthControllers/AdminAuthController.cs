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
    public class AdminAuthController(UserManager<Admin> userManager, IConfiguration configuration, SignInManager<Admin> signInManager) : ControllerBase
    {
        private readonly UserManager<Admin> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly SignInManager<Admin> _signInManager = signInManager;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminSettings loginDto)
        {
            var admin = await _userManager.FindByNameAsync(loginDto.Username);
            if (admin == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var result = await _signInManager.PasswordSignInAsync(admin, loginDto.Password, false, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid credentials" });

            // Get user roles
            var roles = await _userManager.GetRolesAsync(admin);

            // Generate tokens
            var jwtToken = TokenHelper.GenerateJwtToken(admin, _configuration, roles);
            var refreshToken = TokenHelper.GenerateRefreshToken();

            // Save refresh token in DB
            admin.RefreshToken = refreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(admin);

            return Ok(new
            {
                token = jwtToken,
                refreshToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshRequestDto refreshDto)
        {
            var admin = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshDto.RefreshToken);
            if (admin == null || admin.RefreshTokenExpiry < DateTime.UtcNow)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            // Get user roles
            var roles = await _userManager.GetRolesAsync(admin);

            // Generate new tokens
            var newJwtToken = TokenHelper.GenerateJwtToken(admin, _configuration, roles);
            var newRefreshToken = TokenHelper.GenerateRefreshToken();

            // Update refresh token in DB
            admin.RefreshToken = newRefreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(admin);

            return Ok(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }
    }
}
