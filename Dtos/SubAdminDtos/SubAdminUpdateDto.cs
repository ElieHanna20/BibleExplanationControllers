namespace BibleExplanationControllers.Dtos.SubAdminDtos
{
    public class SubAdminUpdateDto
    {
        public string? Username { get; set; } // Optional: Only update if provided
        public string? Password { get; set; } // Optional: Only update if provided
        public bool? CanChangeBooksData { get; set; } // Optional: Only update if provided
    }
}
