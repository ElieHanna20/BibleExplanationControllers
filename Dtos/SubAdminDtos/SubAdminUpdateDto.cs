namespace BibleExplanationControllers.Dtos.SubAdminDtos
{
    public class SubAdminUpdateDto
    {
        public string? Username { get; set; } 
        public string? Password { get; set; } 
        public bool? CanChangeBooksData { get; set; } 
    }
}
