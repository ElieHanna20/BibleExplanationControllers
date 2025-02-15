namespace BibleExplanationControllers.Dtos.SubAdminDtos
{
    public class SubAdminDetailsDto
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public bool CanChangeBooksData { get; set; }
    }
}
