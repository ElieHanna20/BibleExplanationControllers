using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.Bible
{
    public class Book
    {
        [Key]
        public int Id { get; set; } // Primary key (auto-incremented by default)

        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Chapter> Chapters { get; set; } = [];
    }
}

