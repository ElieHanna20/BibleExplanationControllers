namespace BibleExplanationControllers.Dtos.AuthDtos
{
    public class AuthLoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
