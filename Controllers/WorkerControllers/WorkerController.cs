using BibleExplanationControllers.Dtos.WorkerDtos;
using BibleExplanationControllers.Mappers;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.WorkerControllers
{
    [Route("api/workers")]
    [ApiController]
    [Authorize(Roles = "Admin,SubAdmin")]
    public class WorkerController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public WorkerController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWorker([FromBody] WorkerCreateDto dto)
        {
            // Get the current user and ensure it's either an Admin or SubAdmin.
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized("Invalid credentials");

            // For creation, we'll require that the current user is a SubAdmin
            // (as per your business logic, Admins are not allowed to create workers directly)
            if (!(currentUser is SubAdmin subAdmin))
                return Forbid("Only SubAdmins can create workers");

            // Create a new Worker instance using the mapper, setting SubAdminId to the current SubAdmin's Id.
            var worker = dto.ToWorker(subAdmin.Id);

            var result = await _userManager.CreateAsync(worker, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Worker created successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(string id)
        {
            var worker = await _userManager.Users
                .OfType<Worker>()
                .FirstOrDefaultAsync(w => w.Id == id);
            if (worker == null)
                return NotFound("Worker not found");

            var result = await _userManager.DeleteAsync(worker);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Worker deleted successfully" });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateWorker(string id, [FromBody] WorkerUpdateDto dto)
        {
            var worker = await _userManager.Users
                .OfType<Worker>()
                .FirstOrDefaultAsync(w => w.Id == id);
            if (worker == null)
                return NotFound("Worker not found");

            // Get the current user to check permission (should be a SubAdmin or Admin)
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized("Invalid credentials");

            if (currentUser is SubAdmin subAdmin)
            {
                // Only allow changing CanChangeBooksData if the SubAdmin is permitted
                if (!subAdmin.CanChangeBooksData && dto.CanChangeBooksData == true)
                    return Forbid("You are not allowed to grant CanChangeBooksData permission.");
            }
            // Update worker fields via the mapper
            worker.UpdateWorkerFromDto(dto);

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(worker);
                var passwordResult = await _userManager.ResetPasswordAsync(worker, resetToken, dto.Password);
                if (!passwordResult.Succeeded)
                    return BadRequest(passwordResult.Errors);
            }

            var updateResult = await _userManager.UpdateAsync(worker);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors);

            return Ok(new { message = "Worker updated successfully" });
        }

        [HttpGet]
        [Authorize(Roles = "SubAdmin")]
        public async Task<IActionResult> GetAllWorkers()
        {
            // Only allow SubAdmins to view their own workers.
            var currentUser = await _userManager.GetUserAsync(User);
            if (!(currentUser is SubAdmin subAdmin))
                return Unauthorized("Invalid SubAdmin");

            var workers = await _userManager.Users
                .OfType<Worker>()
                .Where(w => w.SubAdminId == subAdmin.Id)
                .Select(w => w.ToWorkerResponseDto())
                .ToListAsync();

            return Ok(workers);
        }

        [HttpGet("by-subadmin/{subAdminId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllWorkersBySubAdmin(string subAdminId)
        {
            var subAdmin = await _userManager.Users
                .OfType<SubAdmin>()
                .FirstOrDefaultAsync(s => s.Id == subAdminId);
            if (subAdmin == null)
                return NotFound("SubAdmin not found");

            var workers = await _userManager.Users
                .OfType<Worker>()
                .Where(w => w.SubAdminId == subAdmin.Id)
                .Select(w => w.ToWorkerResponseDto())
                .ToListAsync();

            return Ok(workers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkerById(string id)
        {
            var worker = await _userManager.Users
                .OfType<Worker>()
                .FirstOrDefaultAsync(w => w.Id == id);
            if (worker == null)
                return NotFound("Worker not found");

            return Ok(worker.ToWorkerResponseDto());
        }
    }
}
