using BibleExplanationControllers.Dtos.AdminDtos;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BibleExplanationControllers.Helpers
{
    public class Helpers
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Helpers(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Seeds the Admin user using only Username and Password from appsettings.json.
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

            var user = await _userManager.FindByNameAsync(adminSettings.Username);
            Admin admin;

            if (user == null)
            {
                // Create new admin. Because Admin extends AppUser, we create an Admin instance.
                admin = new Admin { UserName = adminSettings.Username };
                var createResult = await _userManager.CreateAsync(admin, adminSettings.Password);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errors}");
                }

                // Assign the "Admin" role
                await _userManager.AddToRoleAsync(admin, "Admin");
                Console.WriteLine("Admin user created successfully.");
            }
            else
            {
                // Cast the found user to Admin; if the cast fails, it indicates a configuration issue.
                admin = user as Admin;
                if (admin == null)
                {
                    throw new Exception("The user exists but is not of type Admin.");
                }

                // If the admin exists, update its password if needed
                var passwordValid = await _userManager.CheckPasswordAsync(admin, adminSettings.Password);
                if (!passwordValid)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(admin);
                    var resetResult = await _userManager.ResetPasswordAsync(admin, token, adminSettings.Password);
                    if (!resetResult.Succeeded)
                    {
                        var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to update admin password: {errors}");
                    }
                    Console.WriteLine("Admin password updated.");
                }

                // Ensure the admin is in the "Admin" role
                if (!await _userManager.IsInRoleAsync(admin, "Admin"))
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
