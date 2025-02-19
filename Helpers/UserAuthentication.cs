using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BibleExplanationControllers.Data;
using BibleExplanationControllers.Models.User; // Ensure your Admin, SubAdmin, etc. are here

namespace BibleExplanationControllers.Helpers
{
    public class UserAuthentication
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthentication(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// Signs in the user using cookie authentication and stores the username in session.
        /// </summary>
        public async Task<bool> LoginAsync(User user, string role, HttpContext httpContext)
        {
            // Create authentication claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false, // Session-based cookie (expires when browser closes)
                AllowRefresh = true,
            };

            // Sign in using cookie authentication
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Store the username in session
            httpContext.Session.SetString("Username", user.Username);

            return true;
        }

        /// <summary>
        /// Signs out the user and clears the session.
        /// </summary>
        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            httpContext.Session.Clear();
        }

        /// <summary>
        /// Retrieves the current username from session.
        /// </summary>
        public string? GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("Username");
        }

        /// <summary>
        /// Checks if a user of type T (e.g. Admin, SubAdmin, Worker) is authenticated based on the session.
        /// </summary>
        public async Task<bool> IsAuthenticatedAsync<T>(string sessionKey, DbContext context) where T : class
        {
            var username = _httpContextAccessor.HttpContext?.Session.GetString(sessionKey);
            if (string.IsNullOrEmpty(username))
                return false;

            // Ensure that T has a property "Username" (this assumes your user models include such a property)
            return await context.Set<T>().AnyAsync(u => EF.Property<string>(u, "Username") == username);
        }

        /// <summary>
        /// Retrieves the authenticated Admin from the database using the username stored in session.
        /// </summary>
        public async Task<Admin?> GetAuthenticatedAdminAsync(AuthDbContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            var username = httpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return null;

            return await context.Admins.FirstOrDefaultAsync(a => a.Username == username);
        }
    }
}
