using BibleExplanationControllers.Models.Bible;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class Worker : AppUser
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
