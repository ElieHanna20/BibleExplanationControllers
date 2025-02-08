using System.ComponentModel.DataAnnotations;

namespace BibleExplanationControllers.Dtos.AuthDtos
{
    public class TokenRefreshRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
