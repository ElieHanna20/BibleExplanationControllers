using BibleExplanationControllers.Models.Bible;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class Worker : IdentityUser
    {
        public bool CanChangeBooksData { get; set; }

        [ForeignKey("SubAdmin")]
        public string SubAdminId { get; set; } = string.Empty;

        [JsonIgnore]
        public SubAdmin SubAdmin { get; set; } = default!;

        [JsonIgnore]
        public ICollection<Explanation> Explanations { get; set; } = [];
    }
}
