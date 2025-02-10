namespace BibleExplanationControllers.Dtos.SubAdminDtos
{
    public class SubAdminDetailsDto
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public bool CanChangeBooksData { get; set; }
        public required string AdminId { get; set; }
    }
}
