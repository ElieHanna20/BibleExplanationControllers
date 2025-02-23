using BibleExplanationControllers.Dtos.ExplanationDtos;
using BibleExplanationControllers.Models.Bible;

namespace BibleExplanationControllers.Mappers
{
    public static class ExplanationMapper
    {
        // Convert CreateExplanationRequestDto to Explanation model
        public static Explanation ToExplanationModel(this CreateExplanationRequestDto dto)
        {
            return new Explanation
            {
                Content = dto.Content,
                SubtitleId = dto.SubtitleId,
                SubAdminId = dto.SubAdminId,
                WorkerId = dto.WorkerId
            };
        }

        // Convert Explanation model to ExplanationDto for returning data
        public static ExplanationDto ToExplanationDto(this Explanation explanation)
        {
            return new ExplanationDto
            {
                Id = explanation.Id,
                Content = explanation.Content,
                SubtitleId = explanation.SubtitleId,
                SubAdminId = explanation.SubAdminId,
                WorkerId = explanation.WorkerId
            };
        }

        // Update Explanation model from UpdateExplanationRequestDto
        public static void UpdateExplanationFromDto(this Explanation explanation, UpdateExplanationRequestDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Content))
                explanation.Content = dto.Content;

            if (dto.SubtitleId.HasValue)
                explanation.SubtitleId = dto.SubtitleId;

            if (dto.SubAdminId.HasValue)
                explanation.SubAdminId = dto.SubAdminId;

            if (dto.WorkerId.HasValue)
                explanation.WorkerId = dto.WorkerId;
        }
    }
}
