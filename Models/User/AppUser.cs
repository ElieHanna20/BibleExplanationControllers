using Microsoft.AspNetCore.Identity;

namespace BibleExplanationControllers.Models.User
{
    public class AppUser : IdentityUser

    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
