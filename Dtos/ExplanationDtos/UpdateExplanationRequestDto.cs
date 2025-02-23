namespace BibleExplanationControllers.Dtos.ExplanationDtos
{
    public class UpdateExplanationRequestDto
    {
        public string? Content { get; set; } // Explanation content

        public int? SubtitleId { get; set; } // Foreign Key: Subtitle

        public Guid? SubAdminId { get; set; } // Foreign Key: SubAdmin

        public Guid? WorkerId { get; set; } // Foreign Key: Worker
    }
}
