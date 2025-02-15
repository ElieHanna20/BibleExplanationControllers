using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BibleExplanationControllers.Helpers
{
    public static class JwtServiceExtensions
    {

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Enforce HTTPS in production
                    options.RequireHttpsMetadata = !Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development");
                    options.SaveToken = false; // Not needed since we're handling tokens manually

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),

                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],

                        ValidateAudience = true, // Set to true if you're validating the audience
                        ValidAudience = configuration["Jwt:Audience"],

                        ValidateLifetime = true, // Ensure token hasn't expired
                        ClockSkew = TimeSpan.Zero // Optional: Reduce clock skew
                    };
                });

            return services;
        }
    }
}
