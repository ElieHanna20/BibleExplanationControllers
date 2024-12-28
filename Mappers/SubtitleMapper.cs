using BibleExplanationControllers.Dtos.SubtitleDtos;
using BibleExplanationControllers.Models.Bible;

namespace BibleExplanationControllers.Mappers
{
    public static class SubtitleMapper
    {
        public static SubtitleDto ToSubtitleDto(this Subtitle subtitleModel) => new()
        {
            Id = subtitleModel.Id,
            SubtitleName = subtitleModel.SubtitleName,
        };

        public static Subtitle ToSubtitleModelFromCreateDto(this CreateSubtitleRequestDto subtitleDto, int chapterId) => new()
        {
            SubtitleName = subtitleDto.SubtitleName,
            ChapterId = chapterId
        };


        public static void UpdateSubtitleFromDto(this Subtitle subtitle, UpdateSubtitleRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(subtitle, nameof(subtitle));
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            // Update properties
            subtitle.SubtitleName = dto.SubtitleName;
        }

    }
}
