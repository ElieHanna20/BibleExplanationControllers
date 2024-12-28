using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BibleExplanationControllers.Models.Bible
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; } // Primary Key for EF Core internal use

        [Required]
        [Range(1, 255)]  // Chapter numbers are 1-based
        public byte ChapterNumber { get; set; } // Name of the chapter as a number


        [ForeignKey("Subtitle")]
        public int BookId { get; set; } // Foreign Key: Links to Book

        [JsonIgnore]
        public Book? Book { get; set; } // Navigation property for  the related book

        [JsonIgnore]
        public ICollection<Subtitle> Subtitles { get; set; } = []; // Subtitles in the chapter

        [JsonIgnore]
        public ICollection<Verse> Verses { get; set; } = []; // Verses in the chapter
    }
}
