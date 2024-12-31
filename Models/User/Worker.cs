using BibleExplanationControllers.Models.Bible;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class Worker : IdentityUser<int>
    {
        [Key]
        public override int Id { get; set; }

        [Required]
        public override string? UserName { get; set; }

        [Required]
        public override string? PasswordHash { get; set; }

        public bool CanChangeBooksData { get; set; }

        [ForeignKey("SubAdmin")]
        public int SubAdminId { get; set; }

        [JsonIgnore]
        public SubAdmin? SubAdmin { get; set; }

        [JsonIgnore]
        public ICollection<Explanation> Explanations { get; set; } = [];
    }
}
