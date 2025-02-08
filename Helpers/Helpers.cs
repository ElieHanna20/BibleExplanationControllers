using BibleExplanationControllers.Dtos.AdminDtos;
using BibleExplanationControllers.Models.User;
using Microsoft.AspNetCore.Identity;


namespace BibleExplanationControllers.Helpers
{
    public static class Helpers
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Admin>>();

            var adminSettings = configuration.GetSection("Admin").Get<AdminSettings>();

            if (adminSettings == null || string.IsNullOrWhiteSpace(adminSettings.Username) || string.IsNullOrWhiteSpace(adminSettings.Password))
            {
                throw new Exception("Admin settings are missing or invalid in appsettings.json.");
            }

            var admin = await userManager.FindByNameAsync(adminSettings.Username);

            if (admin == null)
            {
                admin = new Admin
                {
                    UserName = adminSettings.Username,
                    Email = "admin@example.com" // Ensure an email is set if needed
                };

                var createResult = await userManager.CreateAsync(admin, adminSettings.Password);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errors}");
                }

                Console.WriteLine("Admin user created successfully.");
            }
            else
            {
                // Update admin password if changed
                var passwordValid = await userManager.CheckPasswordAsync(admin, adminSettings.Password);
                if (!passwordValid)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(admin);
                    var resetResult = await userManager.ResetPasswordAsync(admin, token, adminSettings.Password);
                    if (!resetResult.Succeeded)
                    {
                        var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to update admin password: {errors}");
                    }

                    Console.WriteLine("Admin password updated.");
                }
            }
        }
    }
}
