namespace BibleExplanationControllers.Dtos.WorkerDtos
{
    public class WorkerDetailsDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public bool? CanChangeBooksData { get; set; }
    }
}
