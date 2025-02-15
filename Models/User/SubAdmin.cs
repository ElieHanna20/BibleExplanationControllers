using BibleExplanationControllers.Models.Bible;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.User
{
    public class SubAdmin : AppUser
    {
        public bool CanChangeBooksData { get; set; }

        [ForeignKey("Admin")]
        public Guid AdminId { get; set; }

        [ForeignKey("AdminId")]
        [JsonIgnore]
        public Admin? Admin { get; set; } = default!;

        [JsonIgnore]
        public ICollection<Worker> Workers { get; set; } = [];

        [JsonIgnore]
        public ICollection<Explanation> Explanations { get; set; } = [];
    }
}
