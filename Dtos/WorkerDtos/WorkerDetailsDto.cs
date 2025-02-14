namespace BibleExplanationControllers.Dtos.WorkerDtos
{
    public class WorkerDetailsDto
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public bool? CanChangeBooksData { get; set; }
    }
}
