namespace BibleExplanationControllers.Dtos.SubAdminDtos
{
    public class SubAdminCreateDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool CanChangeBooksData { get; set; }
    }
}
