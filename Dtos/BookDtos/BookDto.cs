using BibleExplanationControllers.Models.Bible;
using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.BookDtos
{
    public class BookDto
    {
        [Required]
        [MaxLength(255)]
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Id { get; set; }

    }
}
