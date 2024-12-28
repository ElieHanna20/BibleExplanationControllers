using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.VerseDtos
{
    public class CreateVerseRequestDto
    {
        [Required]
        public required string Text { get; set; }
    }
}
