using BibleExplanationControllers.Dtos.VerseDtos;
using BibleExplanationControllers.Models.Bible;

namespace BibleExplanationControllers.Mappers
{
    public static class VerseMapper
    {
        public static VerseDto ToVerseDto(this Verse verseModel) => new()
        {
            Id = verseModel.Id,
            Text = verseModel.Text,
        };

        public static Verse ToSubtitleModelFromCreateDto(this CreateVerseRequestDto verseDto, int subtitleId) => new()
        {
            Text = verseDto.Text,
            SubtitleId = subtitleId
        };


        public static void UpdateVerseDtoFromDto(this Verse verse, UpdateVerseRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(verse, nameof(verse));
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            // Update properties
            verse.Text = dto.Text;
        }

    }
}
