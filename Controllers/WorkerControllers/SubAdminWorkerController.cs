using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.WorkerDtos;
using BibleExplanationControllers.Helpers;
using BibleExplanationControllers.Mappers;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.WorkerControllers
{
    [Route("api/subadmins/workers")]
    [ApiController]
    public class SubAdminWorkerController(AuthDbContext context, PasswordHasher passwordHasher, UserAuthentication userAuthentication) : ControllerBase
    {
        private readonly AuthDbContext _context = context;
        private readonly PasswordHasher _passwordHasher = passwordHasher;
        private readonly UserAuthentication _userAuthentication = userAuthentication;

        private async Task<SubAdmin?> GetSubAdminAsync()
        {
            var currentUsername = _userAuthentication.GetCurrentUsername();
            if (string.IsNullOrWhiteSpace(currentUsername)) return null;

            return await _context.SubAdmins.FirstOrDefaultAsync(a => a.Username == currentUsername);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWorker([FromBody] WorkerCreateDto dto)
        {
            var subAdmin = await GetSubAdminAsync();
            if (subAdmin == null || !await _userAuthentication.IsAuthenticatedAsync<SubAdmin>("Username", _context))
                return Unauthorized("Unauthorized. Only SubAdmins can create Workers.");

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "Username already exists" });

            // Validate plain text password BEFORE hashing
            if (!PasswordValidator.IsValid(dto.Password, out string errorMessage))
            {
                return BadRequest(new { message = errorMessage });
            }

            // Create a new Worker instance
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
            var subAdmin = await GetSubAdminAsync();
            if (subAdmin == null || !await _userAuthentication.IsAuthenticatedAsync<SubAdmin>("Username", _context))
                return Unauthorized("Unauthorized");

            var workers = await _context.Workers
                .Where(w => w.SubAdminId == subAdmin.Id)
                .Select(w => w.ToWorkerResponseDto())
                .ToListAsync();

            return Ok(workers);
        }

        [HttpGet("my-workers/{workerId}")]
        public async Task<IActionResult> GetWorkerById(Guid workerId)
        {
            var subAdmin = await GetSubAdminAsync();
            if (subAdmin == null || !await _userAuthentication.IsAuthenticatedAsync<SubAdmin>("Username", _context))
                return Unauthorized("Unauthorized");

            var worker = await _context.Workers
                .Where(w => w.Id == workerId && w.SubAdminId == subAdmin.Id)
                .Select(w => w.ToWorkerResponseDto())
                .FirstOrDefaultAsync();

            if (worker == null)
                return NotFound("Worker not found or does not belong to this SubAdmin.");

            return Ok(worker);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateWorker(Guid id, [FromBody] WorkerUpdateDto dto)
        {
            var subAdmin = await GetSubAdminAsync();

            if (subAdmin == null || !await _userAuthentication.IsAuthenticatedAsync<SubAdmin>("Username", _context))
            {
                return Unauthorized(new { message = "Unauthorized. Only the assigned SubAdmin can update this worker." });
            }

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
                return NotFound(new { message = "Worker not found" });

            // Centralized permission check: If SubAdmin is not the owner of the Worker, deny access
            if (worker.SubAdminId != subAdmin.Id || (dto.CanChangeBooksData == true && !subAdmin.CanChangeBooksData))
            {
                return Unauthorized(new { message = "You do not have the required permissions to update this worker." });
            }

            // Update worker fields via the mapper
            worker.UpdateWorkerFromDto(dto);

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                // Validate password complexity
                if (!PasswordValidator.IsValid(dto.Password, out string errorMessage))
                {
                    return BadRequest(new { message = errorMessage });
                }

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
            var subAdmin = await GetSubAdminAsync();
            if (subAdmin == null || !await _userAuthentication.IsAuthenticatedAsync<SubAdmin>("Username", _context))
                return Unauthorized("Unauthorized");

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
                return NotFound("Worker not found");

            // SubAdmin can only delete their own workers
            if (worker.SubAdminId != subAdmin.Id)
                return Forbid("You are not authorized to delete this worker.");

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Worker deleted successfully" });
        }
    }
}
