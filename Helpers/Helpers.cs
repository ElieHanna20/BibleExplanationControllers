using System;
using System.Linq;
using System.Threading.Tasks;
using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.AdminDtos;
using BibleExplanationControllers.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BibleExplanationControllers.Helpers
{
    public class Helpers
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHasher _passwordHasher;

        public Helpers(AuthDbContext context, PasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Seeds the Admin user using Username and Password from appsettings.json.
        /// If the Admin user exists, updates its password if necessary.
        /// </summary>
        public async Task SeedAdminAsync(IConfiguration configuration)
        {
            var adminSettings = configuration.GetSection("Admin").Get<AdminSettings>();

            if (adminSettings == null ||
                string.IsNullOrWhiteSpace(adminSettings.Username) ||
                string.IsNullOrWhiteSpace(adminSettings.Password))
            {
                throw new Exception("Admin settings are missing or invalid in appsettings.json.");
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == adminSettings.Username);

            if (admin == null)
            {
                // Create new admin
                admin = new Admin
                {
                    Id = Guid.NewGuid(),
                    Username = adminSettings.Username,
                    PasswordHash = _passwordHasher.HashPassword(adminSettings.Password)
                };

                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();
                Console.WriteLine("Admin user created successfully.");
            }
            else
            {
                // Update password if necessary
                if (!_passwordHasher.VerifyPassword(admin.PasswordHash, adminSettings.Password))
                {
                    admin.PasswordHash = _passwordHasher.HashPassword(adminSettings.Password);
                    _context.Admins.Update(admin);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Admin password updated.");
                }
            }
        }
    }
}
