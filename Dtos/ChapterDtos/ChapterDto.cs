using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.ChapterDtos
{
    public class ChapterDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(1, 255)]
        public byte ChapterNumber { get; set; }
    }
}
