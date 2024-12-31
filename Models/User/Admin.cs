using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class Admin : IdentityUser<int>
    {
        [Key]
        public override int Id { get; set; }

        [Required]
        public override string? UserName { get; set; }

        [Required]
        public override string? PasswordHash { get; set; }

        [JsonIgnore]
        public ICollection<SubAdmin> SubAdmins { get; set; } = [];
    }
}
