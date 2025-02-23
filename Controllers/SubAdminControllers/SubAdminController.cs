using BibleExplanationControllers.Dtos.SubAdminDtos;
using BibleExplanationControllers.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibleExplanationControllers.Helpers;
using BibleExplanationControllers.Data;
using BibleExplanationControllers.Models.User;

namespace BibleExplanationControllers.Controllers.SubAdminControllers
{
    [Route("api/subadmins")]
    [ApiController]
    public class SubAdminController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly UserAuthentication _userAuthentication;

        public SubAdminController(AuthDbContext context, PasswordHasher passwordHasher, UserAuthentication userAuthentication)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _userAuthentication = userAuthentication;
        }

        private async Task<Admin?> GetAuthenticatedAdminAsync()
        {
            return await _userAuthentication.GetAuthenticatedAdminAsync(_context);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSubAdmin([FromBody] SubAdminCreateDto dto)
        {
            var admin = await GetAuthenticatedAdminAsync();
            if (admin == null)
                return Unauthorized(new { message = "Unauthorized. Only Admins can create SubAdmins." });

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { message = "Username already exists" });

            // Validate plain text password BEFORE hashing
            if (!PasswordValidator.IsValid(dto.Password, out string errorMessage))
            {
                return BadRequest(new { message = errorMessage });
            }

            // Create SubAdmin and hash password
            var subAdmin = dto.ToSubAdmin(admin.Id);
            subAdmin.PasswordHash = _passwordHasher.HashPassword(dto.Password);

            // Save to database
            _context.SubAdmins.Add(subAdmin);
            await _context.SaveChangesAsync();

            return Ok(new { message = "SubAdmin created successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubAdmin(Guid id)
        {
            var admin = await GetAuthenticatedAdminAsync();
            if (admin == null)
                return Unauthorized(new { message = "Unauthorized" });

            // Fetch the SubAdmin including related workers (users)
            var subAdmin = await _context.SubAdmins
                .Include(sa => sa.Workers) // Assuming 'Users' is the navigation property for workers
                .FirstOrDefaultAsync(sa => sa.Id == id);

            if (subAdmin == null)
                return NotFound(new { message = "SubAdmin not found" });

            // Delete all related workers (users) before deleting SubAdmin
            _context.Users.RemoveRange(subAdmin.Workers);

            // Now, delete the SubAdmin
            _context.SubAdmins.Remove(subAdmin);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { message = "SubAdmin and related workers deleted successfully" });
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSubAdmin(Guid id, [FromBody] SubAdminUpdateDto dto)
        {
            var admin = await GetAuthenticatedAdminAsync();
            if (admin == null)
                return Unauthorized(new { message = "Unauthorized" });

            var subAdmin = await _context.SubAdmins
                .Include(sa => sa.Workers) // Include related Workers
                .FirstOrDefaultAsync(sa => sa.Id == id);

            if (subAdmin == null)
                return NotFound(new { message = "SubAdmin not found" });

            // Handle CanChangeBooksData permission update
            if (dto.CanChangeBooksData.HasValue) // Only update if a new value is provided
            {
                if (!dto.CanChangeBooksData.Value) // If changed to false, revoke for all Workers
                {
                    subAdmin.CanChangeBooksData = false;
                    foreach (var worker in subAdmin.Workers)
                    {
                        worker.CanChangeBooksData = false;
                    }
                }
                else
                {
                    subAdmin.CanChangeBooksData = true; // Allow changing books data
                }
            }

            // Update other fields
            subAdmin.UpdateSubAdminFromDto(dto);

            // Update password only if a new password is provided
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                // Validate password complexity
                if (!PasswordValidator.IsValid(dto.Password, out string errorMessage))
                {
                    return BadRequest(new { message = errorMessage });
                }

                subAdmin.PasswordHash = _passwordHasher.HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "SubAdmin updated successfully" });
        }




        [HttpGet]
        public async Task<IActionResult> GetAllSubAdmins()
        {
            var admin = await GetAuthenticatedAdminAsync();
            if (admin == null)
                return Unauthorized(new { message = "Unauthorized" });

            var subAdmins = await _context.SubAdmins
                .Select(sa => sa.ToSubAdminDetailsDto())
                .ToListAsync();

            return Ok(subAdmins);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubAdminById(Guid id)
        {
            var admin = await GetAuthenticatedAdminAsync();
            if (admin == null)
                return Unauthorized(new { message = "Unauthorized" });

            var subAdmin = await _context.SubAdmins.FindAsync(id);
            if (subAdmin == null)
                return NotFound(new { message = "SubAdmin not found" });

            return Ok(subAdmin.ToSubAdminDetailsDto());
        }
    }
}
