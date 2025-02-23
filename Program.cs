using BibleExplanationControllers.Data;
using BibleExplanationControllers.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "v1" });
});

// Configure DbContext
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnection")));
builder.Services.AddDbContext<BibleDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BibleConnection")));

// Register PasswordHasher and Helpers
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<Helpers>();

// Register UserAuthentication (if needed for other purposes)
builder.Services.AddScoped<UserAuthentication>();

// Configure Cookie Authentication (only one cookie now)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "YourAppCookie";
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/access-denied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.MaxAge = TimeSpan.FromHours(1); // Set expiration time
        options.SlidingExpiration = true;  // Sliding expiration
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("CanChangeBooksData", policy =>
        policy.Requirements.Add(new CanChangeBooksDataRequirement()));

builder.Services.AddSingleton<IAuthorizationHandler, CanChangeBooksDataHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
