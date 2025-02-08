using BibleExplanationControllers.Helpers;
using BibleExplanationControllers.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BibleExplanationControllers.Models.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT Bearer token"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add DbContexts
builder.Services.AddDbContext<BibleDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BibleConnection")));
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnection")));

// Add Identity services
builder.Services.AddIdentity<Admin, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Add JWT Authentication service
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

// Ensure the admin user is seeded BEFORE the app starts
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var configuration = services.GetRequiredService<IConfiguration>();

    // Call the seeding function
    await Helpers.SeedAdminAsync(services, configuration);
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error during seeding: {ex.Message}");
    return; // Stop app from running
}


// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseHttpsRedirection();

// Configure CORS
CorsConfig.ConfigureCors(app, builder.Configuration);

// Add authentication middleware
app.UseAuthentication();

// Add authorization middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
