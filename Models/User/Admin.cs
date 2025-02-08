using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class Admin : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        [JsonIgnore]
        public ICollection<SubAdmin> SubAdmins { get; set; } = [];
    }
}
