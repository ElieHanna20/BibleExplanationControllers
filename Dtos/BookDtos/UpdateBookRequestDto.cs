using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.BookDtos
{
    public class UpdateBookRequestDto
    {
        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public required string Name { get; set; }
    }
}
