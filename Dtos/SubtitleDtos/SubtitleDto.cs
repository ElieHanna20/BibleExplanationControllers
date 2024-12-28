using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.SubtitleDtos
{
    public class SubtitleDto
    {
        [Required]
        public int Id { get; set; } // Primary Key for EF Core internal use

        [Required]
        [MaxLength(200)] // Limiting the length of the subtitle name
        public string SubtitleName { get; set; } = string.Empty; // Name of the subtitle
    }
}
