using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.WorkerDtos;
using BibleExplanationControllers.Helpers;
using BibleExplanationControllers.Mappers;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.WorkerControllers
{
    [Route("api/admin/workers")]
    [ApiController]
    public class AdminWorkerController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly UserAuthentication _userAuthentication;

        public AdminWorkerController(AuthDbContext context, PasswordHasher passwordHasher, UserAuthentication userAuthentication)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userAuthentication = userAuthentication;
        }

        private async Task<Admin?> GetAdminAsync()
        {
            var currentUsername = _userAuthentication.GetCurrentUsername();
            if (string.IsNullOrWhiteSpace(currentUsername)) return null;

            return await _context.Admins.FirstOrDefaultAsync(a => a.Username == currentUsername);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWorker([FromBody] WorkerCreateDto dto)
        {
            var admin = await GetAdminAsync();
            if (admin == null || !await _userAuthentication.IsAuthenticatedAsync<Admin>("Username", _context))
                return Unauthorized("Unauthorized. Only Admins can create Workers.");

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
            var admin = await GetAdminAsync();
            if (admin == null || !await _userAuthentication.IsAuthenticatedAsync<Admin>("Username", _context))
                return Unauthorized("Unauthorized");

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
            var admin = await GetAdminAsync();
            if (admin == null || !await _userAuthentication.IsAuthenticatedAsync<Admin>("Username", _context))
                return Unauthorized("Unauthorized");

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
            var admin = await GetAdminAsync();
            if (admin == null || !await _userAuthentication.IsAuthenticatedAsync<Admin>("Username", _context))
                return Unauthorized("Unauthorized");

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
                return NotFound("Worker not found");

            return Ok(worker.ToWorkerResponseDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(Guid id)
        {
            var admin = await GetAdminAsync();
            if (admin == null || !await _userAuthentication.IsAuthenticatedAsync<Admin>("Username", _context))
                return Unauthorized("Unauthorized");

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
                return NotFound("Worker not found");

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Worker deleted successfully" });
        }
    }
}
