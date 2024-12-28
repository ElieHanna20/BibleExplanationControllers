using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.VerseDtos
{
    public class VerseDto
    {
        public int Id { get; set; }

        [Required]
        public required string Text { get; set; }
    }
}
