using BibleExplanationControllers.Dtos.SubAdminDtos;
using BibleExplanationControllers.Mappers;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.AdminControllers
{
    [Route("api/admin/subadmins")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Ensures only Admins can access
    public class SubAdminController(UserManager<SubAdmin> subAdminManager, UserManager<Admin> adminManager) : ControllerBase
    {
        private readonly UserManager<SubAdmin> _subAdminManager = subAdminManager;
        private readonly UserManager<Admin> _adminManager = adminManager;

        [HttpPost("create")]
        public async Task<IActionResult> CreateSubAdmin([FromBody] SubAdminCreateDto dto)
        {
            var admin = await _adminManager.GetUserAsync(User);
            if (admin == null) return Unauthorized("Invalid Admin");

            var subAdmin = dto.ToSubAdmin(admin.Id);
            var result = await _subAdminManager.CreateAsync(subAdmin, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = "SubAdmin created successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubAdmin(string id)
        {
            var subAdmin = await _subAdminManager.FindByIdAsync(id);
            if (subAdmin == null) return NotFound("SubAdmin not found");

            await _subAdminManager.DeleteAsync(subAdmin);
            return Ok(new { message = "SubAdmin deleted successfully" });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSubAdmin(string id, [FromBody] SubAdminUpdateDto dto)
        {
            var subAdmin = await _subAdminManager.FindByIdAsync(id);
            if (subAdmin == null) return NotFound("SubAdmin not found");

            // Use mapper to update SubAdmin fields
            subAdmin.UpdateSubAdminFromDto(dto);

            // Update password if provided
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var token = await _subAdminManager.GeneratePasswordResetTokenAsync(subAdmin);
                var passwordResult = await _subAdminManager.ResetPasswordAsync(subAdmin, token, dto.Password);

                if (!passwordResult.Succeeded)
                    return BadRequest(passwordResult.Errors);
            }

            var result = await _subAdminManager.UpdateAsync(subAdmin);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = "SubAdmin updated successfully" });
        }


        [HttpGet]
        public async Task<IActionResult> GetAllSubAdmins()
        {
            var subAdmins = await _subAdminManager.Users
                .Select(sa => sa.ToSubAdminDetailsDto())
                .ToListAsync();

            return Ok(subAdmins);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubAdminById(string id)
        {
            var subAdmin = await _subAdminManager.FindByIdAsync(id);
            if (subAdmin == null) return NotFound("SubAdmin not found");

            return Ok(subAdmin.ToSubAdminDetailsDto());
        }
    }
}
