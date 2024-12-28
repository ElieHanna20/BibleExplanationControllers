using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.VerseDtos
{
    public class UpdateVerseRequestDto
    {
        [Required]
        public required string Text { get; set; }
    }
}
