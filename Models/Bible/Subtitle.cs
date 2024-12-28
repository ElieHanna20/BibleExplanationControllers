using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.Bible
{
    public class Subtitle
    {
        [Key]
        public int Id { get; set; } // Primary Key for EF Core internal use

        [Required]
        [MaxLength(200)] // Limiting the length of the subtitle name
        public string SubtitleName { get; set; } = string.Empty; // Name of the subtitle

        [ForeignKey("Chapter")]
        public int ChapterId { get; set; } // Foreign Key: Links to Chapter

        [JsonIgnore]
        public Chapter? Chapter { get; set; } // Navigation property for the related chapter

        [JsonIgnore]
        public ICollection<Verse> Verses { get; set; } = []; // Collection of verses

        [JsonIgnore]
        public ICollection<Explanation> Explanations { get; set; } = []; // Collection of explanations
    }
}
