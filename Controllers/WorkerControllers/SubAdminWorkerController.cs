using BibleExplanationControllers.Dtos.WorkerDtos;
using BibleExplanationControllers.Mappers;
using BibleExplanationControllers.Models.User;
using BibleExplanationControllers.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BibleExplanationControllers.Data;

namespace BibleExplanationControllers.Controllers.WorkerControllers
{
    [Route("api/subadmins/workers")]
    [ApiController]
    [Authorize(Roles = "SubAdmin")]
    public class SubAdminWorkerController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHasher _passwordHasher;

        public SubAdminWorkerController(AuthDbContext context, PasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWorker([FromBody] WorkerCreateDto dto)
        {
            // Get current SubAdmin ID from token
            var currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdString == null)
                return Unauthorized("Invalid credentials");

            if (!Guid.TryParse(currentUserIdString, out var currentUserId))
                return Unauthorized("Invalid user ID");

            // Fetch current SubAdmin from database
            var subAdmin = await _context.SubAdmins.FindAsync(currentUserId);
            if (subAdmin == null)
                return Unauthorized("Invalid SubAdmin");

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "Username already exists" });

            // Create a new Worker instance using the mapper, setting SubAdminId to the current SubAdmin's Id
            var worker = dto.ToWorker(subAdmin.Id);

            // Hash the password
            worker.PasswordHash = _passwordHasher.HashPassword(dto.Password);

            // Add worker to the database
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Worker created successfully" });
        }

        [HttpGet("my-workers")]
        public async Task<IActionResult> GetMyWorkers()
        {
            // Get current SubAdmin ID from token
            var currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdString == null)
                return Unauthorized("Invalid credentials");

            if (!Guid.TryParse(currentUserIdString, out var subAdminId))
                return Unauthorized("Invalid user ID");

            var workers = await _context.Workers
                .Where(w => w.SubAdminId == subAdminId)
                .Select(w => w.ToWorkerResponseDto())
                .ToListAsync();

            return Ok(workers);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateWorker(Guid id, [FromBody] WorkerUpdateDto dto)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
                return NotFound("Worker not found");

            // Get current SubAdmin ID from token
            var currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdString == null)
                return Unauthorized("Invalid credentials");

            if (!Guid.TryParse(currentUserIdString, out var currentUserId))
                return Unauthorized("Invalid user ID");

            var subAdmin = await _context.SubAdmins.FindAsync(currentUserId);
            if (subAdmin == null)
                return Unauthorized("Invalid SubAdmin");

            // SubAdmins can only update their own workers
            if (worker.SubAdminId != currentUserId)
                return Forbid("You are not authorized to update this worker.");

            // Only allow changing CanChangeBooksData if the SubAdmin is permitted
            if (dto.CanChangeBooksData == true && !subAdmin.CanChangeBooksData)
                return Forbid("You are not allowed to grant CanChangeBooksData permission.");

            // Update worker fields via the mapper
            worker.UpdateWorkerFromDto(dto);

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                // Hash and update password
                worker.PasswordHash = _passwordHasher.HashPassword(dto.Password);
            }

            _context.Workers.Update(worker);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Worker updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(Guid id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound("Worker not found");
            }

            // Get current SubAdmin ID from token
            var currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdString == null)
            {
                return Unauthorized("Invalid credentials");
            }

            if (!Guid.TryParse(currentUserIdString, out var currentUserId))
            {
                return Unauthorized("Invalid user ID");
            }

            var subAdmin = await _context.SubAdmins.FindAsync(currentUserId);
            if (subAdmin == null)
            {
                return Unauthorized("Invalid SubAdmin");
            }

            // SubAdmin can only delete their own workers
            if (worker.SubAdminId != currentUserId)
            {
                return Forbid("You are not authorized to delete this worker.");
            }

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Worker deleted successfully" });
        }
    }
}
