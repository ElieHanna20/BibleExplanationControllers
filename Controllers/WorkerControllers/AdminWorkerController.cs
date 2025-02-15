using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.WorkerDtos;
using BibleExplanationControllers.Helpers;
using BibleExplanationControllers.Mappers;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BibleExplanationControllers.Controllers.WorkerControllers
{
    [Route("api/admin/workers")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminWorkerController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHasher _passwordHasher;

        public AdminWorkerController(AuthDbContext context, PasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWorker([FromBody] WorkerCreateDto dto)
        {
            // Get current user ID from token
            var currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserIdString == null)
                return Unauthorized("Invalid credentials");

            if (!Guid.TryParse(currentUserIdString, out var currentUserId))
                return Unauthorized("Invalid user ID");

            // Fetch current user from database
            var currentUser = await _context.Admins.FindAsync(currentUserId);
            if (currentUser == null)
                return Unauthorized("Invalid Admin");

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "Username already exists" });

            if (dto.SubAdminId == null)
                return BadRequest("SubAdminId is required when Admin creates a worker.");

            // Validate the provided SubAdminId
            var subAdmin = await _context.SubAdmins.FindAsync(dto.SubAdminId.Value);
            if (subAdmin == null)
                return NotFound("SubAdmin not found.");

            // Create a new Worker instance
            var worker = dto.ToWorker(dto.SubAdminId.Value);

            // Hash the password
            worker.PasswordHash = _passwordHasher.HashPassword(dto.Password);

            // Add worker to the database
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Worker created successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkers(Guid? subAdminId = null)
        {
            IQueryable<Worker> workersQuery = _context.Workers;

            if (subAdminId.HasValue)
            {
                workersQuery = workersQuery.Where(w => w.SubAdminId == subAdminId.Value);
            }

            var workers = await workersQuery
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkerById(Guid id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
                return NotFound("Worker not found");

            return Ok(worker.ToWorkerResponseDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(Guid id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
                return NotFound("Worker not found");

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Worker deleted successfully" });
        }
    }

}
