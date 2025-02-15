using BibleExplanationControllers.Dtos.SubAdminDtos;
using BibleExplanationControllers.Mappers;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using BibleExplanationControllers.Helpers;
using BibleExplanationControllers.Data;

namespace BibleExplanationControllers.Controllers.SubAdminControllers
{
    [Route("api/subadmins")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SubAdminController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHasher _passwordHasher;

        public SubAdminController(AuthDbContext context, PasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSubAdmin([FromBody] SubAdminCreateDto dto)
        {
            // Get current user ID from token
            var currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdString))
                return Unauthorized("Invalid credentials");

            if (!Guid.TryParse(currentUserIdString, out var currentUserId))
                return Unauthorized("Invalid user ID");

            // Fetch current user from the database
            var currentUser = await _context.Admins.FindAsync(currentUserId);
            if (currentUser == null)
                return Unauthorized("Invalid Admin");

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "Username already exists" });

            // Convert the DTO to a SubAdmin instance
            var subAdmin = dto.ToSubAdmin(currentUser.Id);

            // Hash the password
            subAdmin.PasswordHash = _passwordHasher.HashPassword(dto.Password);

            // Add SubAdmin to the database
            _context.SubAdmins.Add(subAdmin);
            await _context.SaveChangesAsync();

            return Ok(new { message = "SubAdmin created successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubAdmin(Guid id)
        {
            var subAdmin = await _context.SubAdmins.FindAsync(id);
            if (subAdmin == null)
                return NotFound("SubAdmin not found");

            _context.SubAdmins.Remove(subAdmin);
            await _context.SaveChangesAsync();

            return Ok(new { message = "SubAdmin deleted successfully" });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSubAdmin(Guid id, [FromBody] SubAdminUpdateDto dto)
        {
            var subAdmin = await _context.SubAdmins.FindAsync(id);
            if (subAdmin == null)
                return NotFound("SubAdmin not found");

            // Update fields using the mapper extension.
            subAdmin.UpdateSubAdminFromDto(dto);

            // Update password if provided.
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                // Hash and update the password
                subAdmin.PasswordHash = _passwordHasher.HashPassword(dto.Password);
            }

            _context.SubAdmins.Update(subAdmin);
            await _context.SaveChangesAsync();

            return Ok(new { message = "SubAdmin updated successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubAdmins()
        {
            var subAdmins = await _context.SubAdmins
                .Select(sa => sa.ToSubAdminDetailsDto())
                .ToListAsync();

            return Ok(subAdmins);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubAdminById(Guid id)
        {
            var subAdmin = await _context.SubAdmins.FindAsync(id);
            if (subAdmin == null)
                return NotFound("SubAdmin not found");

            return Ok(subAdmin.ToSubAdminDetailsDto());
        }
    }
}
