using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.ChapterDtos
{
    public class CreateChapterRequestDto
    {
        [Required]
        [Range(1, 255)]
        public required byte ChapterNumber { get; set; }
    }
}
