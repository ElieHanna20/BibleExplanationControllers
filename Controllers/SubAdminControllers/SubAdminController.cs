﻿using BibleExplanationControllers.Dtos.SubAdminDtos;
using BibleExplanationControllers.Mappers;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.SubAdminControllers
{
    [Route("api/subadmins")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SubAdminController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public SubAdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSubAdmin([FromBody] SubAdminCreateDto dto)
        {
            // Get the current user and ensure it's an Admin.
            var currentUser = await _userManager.GetUserAsync(User);
            if (!(currentUser is Admin))
                return Unauthorized("Invalid Admin");

            // Convert the DTO to a SubAdmin instance.
            var subAdmin = dto.ToSubAdmin();

            // Create the SubAdmin user.
            var result = await _userManager.CreateAsync(subAdmin, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "SubAdmin created successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubAdmin(string id)
        {
            var subAdmin = await _userManager.Users
                .OfType<SubAdmin>()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (subAdmin == null)
                return NotFound("SubAdmin not found");

            var result = await _userManager.DeleteAsync(subAdmin);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "SubAdmin deleted successfully" });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSubAdmin(string id, [FromBody] SubAdminUpdateDto dto)
        {
            var subAdmin = await _userManager.Users
                .OfType<SubAdmin>()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (subAdmin == null)
                return NotFound("SubAdmin not found");

            // Update fields using the mapper extension.
            subAdmin.UpdateSubAdminFromDto(dto);

            // Update password if provided.
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(subAdmin);
                var passwordResult = await _userManager.ResetPasswordAsync(subAdmin, token, dto.Password);
                if (!passwordResult.Succeeded)
                    return BadRequest(passwordResult.Errors);
            }

            var updateResult = await _userManager.UpdateAsync(subAdmin);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors);

            return Ok(new { message = "SubAdmin updated successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubAdmins()
        {
            var subAdmins = await _userManager.Users
                .OfType<SubAdmin>()
                .Select(sa => sa.ToSubAdminDetailsDto())
                .ToListAsync();

            return Ok(subAdmins);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubAdminById(string id)
        {
            var subAdmin = await _userManager.Users
                .OfType<SubAdmin>()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (subAdmin == null)
                return NotFound("SubAdmin not found");

            return Ok(subAdmin.ToSubAdminDetailsDto());
        }
    }
}
