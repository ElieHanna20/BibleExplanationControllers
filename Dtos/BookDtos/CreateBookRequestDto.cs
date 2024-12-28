using BibleExplanationControllers.Models.Bible;
using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.BookDtos
{
    public class CreateBookRequestDto
    {
        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public required string Name { get; set; }
    }
}
