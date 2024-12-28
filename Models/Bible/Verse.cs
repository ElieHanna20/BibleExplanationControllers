using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.Bible
{
    public class Verse
    {
        [Key]
        public int Id { get; set; } // Primary Key for EF Core internal use

        [Required]
        [Range(1, 255)]  // Verse numbers are 1-based
        public byte VerseNumber { get; set; } // Name of the verse as a number

        public required string Text { get; set; } // The content of the verse

        [ForeignKey("Subtitle")]
        public int SubtitleId { get; set; } // Foreign Key: Links to Subtitle

        [JsonIgnore]
        public Subtitle? Subtitle { get; set; } // Navigation property for the related subtitle
    }
}
