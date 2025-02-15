using BibleExplanationControllers.Data;
using BibleExplanationControllers.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "v1" });

    // Configure JWT authentication for Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. " +
                      "\r\n\r\nEnter 'Bearer' [space] and then your token in the text input below." +
                      "\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

// Configure DbContext
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnection")));
builder.Services.AddDbContext<BibleDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BibleConnection")));


// Register PasswordHasher and Helpers
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<Helpers>();
builder.Services.AddScoped<TokenHelper>(); // Register TokenHelper

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Enforce HTTPS in production
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false; // Not needed since we're handling tokens manually

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true, // Set to true if you are validating the audience
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true, // Ensure token hasn't expired
            ClockSkew = TimeSpan.Zero // Optional: reduce default clock skew of 5 mins
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Admin user before the app starts
var logger = app.Services.GetRequiredService<ILogger<Program>>();
try
{
    using var scope = app.Services.CreateScope();
    var helpers = scope.ServiceProvider.GetRequiredService<Helpers>();
    await helpers.SeedAdminAsync(builder.Configuration);
}
catch (Exception ex)
{
    logger.LogError($"Fatal error during seeding: {ex.Message}");
    return;
}

app.Run();
