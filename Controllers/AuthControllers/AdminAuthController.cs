using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.AdminDtos;
using BibleExplanationControllers.Dtos.AuthDtos;
using BibleExplanationControllers.Helpers;
using BibleExplanationControllers.Models.User;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.AuthControllers
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AdminAuthController(AuthDbContext context, PasswordHasher passwordHasher, TokenHelper tokenHelper) : ControllerBase
    {
        private readonly AuthDbContext _context = context;
        private readonly PasswordHasher _passwordHasher = passwordHasher;
        private readonly TokenHelper _tokenHelper = tokenHelper;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminSettings loginDto)
        {
            // Find the admin by username
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == loginDto.Username);
            if (admin == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // Verify the password
            if (!_passwordHasher.VerifyPassword(admin.PasswordHash, loginDto.Password))
                return Unauthorized(new { message = "Invalid credentials" });

            // Generate tokens
            var accessToken = _tokenHelper.GenerateJwtToken(admin, "Admin");
            var refreshToken = _tokenHelper.GenerateRefreshToken();

            // Save refresh token & expiry in DB
            admin.RefreshToken = refreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshRequestDto refreshDto)
        {
            // Find the admin by refresh token
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.RefreshToken == refreshDto.RefreshToken);
            if (admin == null || admin.RefreshTokenExpiry <= DateTime.UtcNow)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            // Generate new tokens
            var newAccessToken = _tokenHelper.GenerateJwtToken(admin, "Admin");
            var newRefreshToken = _tokenHelper.GenerateRefreshToken();

            // Update refresh token and expiry in DB
            admin.RefreshToken = newRefreshToken;
            admin.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
