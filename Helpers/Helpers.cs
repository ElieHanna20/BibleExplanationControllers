using BibleExplanationControllers.Data;
using BibleExplanationControllers.Models.User;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Helpers
{
    public class Helpers(AuthDbContext context, PasswordHasher passwordHasher)
    {
        private readonly AuthDbContext _context = context;
        private readonly PasswordHasher _passwordHasher = passwordHasher;

        /// <summary>
        /// Seeds the Admin user using Username and Password from appsettings.json.
        /// If the Admin user exists, updates its password if necessary.
        /// </summary>
        public async Task SeedAdminAsync(IConfiguration configuration)
        {
            var adminUsername = configuration["Admin:Username"];
            var adminPassword = configuration["Admin:Password"];

            if (string.IsNullOrWhiteSpace(adminUsername) || string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new Exception("Admin credentials are missing in appsettings.json.");
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == adminUsername);

            if (admin == null)
            {
                // Create new admin
                admin = new Admin
                {
                    Id = Guid.NewGuid(),
                    Username = adminUsername,
                    PasswordHash = _passwordHasher.HashPassword(adminPassword)
                };

                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();
                Console.WriteLine("Admin user created successfully.");
            }
            else
            {
                // Update password if necessary
                if (!_passwordHasher.VerifyPassword(admin.PasswordHash, adminPassword))
                {
                    admin.PasswordHash = _passwordHasher.HashPassword(adminPassword);
                    _context.Admins.Update(admin);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Admin password updated.");
                }
            }
        }
    }
}
