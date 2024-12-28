using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.ChapterDtos
{
    public class UpdateChapterRequestDto
    {
        [Required]
        [Range(1, 255)]
        public required byte ChapterNumber { get; set; }

    }
}
