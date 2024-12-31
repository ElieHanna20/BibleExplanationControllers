using BibleExplanationControllers.Models.Bible;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class SubAdmin : IdentityUser<int>
    {
        [Key]
        public override int Id { get; set; }

        [Required]
        public override string? UserName { get; set; }

        [Required]
        public override string? PasswordHash { get; set; }

        public bool CanChangeBooksData { get; set; }

        [ForeignKey("Admin")]
        public int AdminId { get; set; }

        [JsonIgnore]
        public Admin? Admin { get; set; }

        [JsonIgnore]
        public ICollection<Worker> Workers { get; set; } = [];

        [JsonIgnore]
        public ICollection<Explanation> Explanations { get; set; } = [];
    }
}
