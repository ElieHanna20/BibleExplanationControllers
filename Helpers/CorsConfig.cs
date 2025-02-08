namespace BibleExplanationControllers.Helpers
{
    public static class CorsConfig
    {
        public static void ConfigureCors(WebApplication app, IConfiguration configuration)
        {
            var environment = app.Environment.EnvironmentName; // Get the current environment (Development or Production)
            var allowedOrigins = configuration.GetSection($"CorsSettings:{environment}").Get<string[]>();

            if (allowedOrigins != null && allowedOrigins.Length > 0)
            {
                app.UseCors(builder =>
                {
                    builder.WithOrigins(allowedOrigins) // Use the origins defined in the environment
                        .AllowAnyMethod()                // Allow any HTTP method (GET, POST, etc.)
                        .AllowAnyHeader();               // Allow any HTTP headers
                });
            }
            else
            {
                Console.WriteLine("No allowed origins specified for CORS.");
            }
        }
    }
}
