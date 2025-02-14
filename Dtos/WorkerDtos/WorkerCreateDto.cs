namespace BibleExplanationControllers.Dtos.WorkerDtos
{
    public class WorkerCreateDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool? CanChangeBooksData { get; set; } 
    }
}
