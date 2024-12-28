using BibleExplanationControllers.Dtos.BookDtos;
using BibleExplanationControllers.Dtos.ChapterDtos;
using BibleExplanationControllers.Models.Bible;
using System.Net;

namespace BibleExplanationControllers.Mappers
{
    public static class ChapterMapper
    {
        public static ChapterDto ToChapterDto(this Chapter chapterModel) => new()
        {
            Id = chapterModel.Id,
            ChapterNumber = chapterModel.ChapterNumber,
        };


        public static Chapter ToChapterModelFromCreateDto(this CreateChapterRequestDto chapterDto, int bookId) => new()
        {
            ChapterNumber = chapterDto.ChapterNumber,
            BookId = bookId
        };

        public static void UpdateChapterFromDto(this Chapter chapter, UpdateChapterRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(chapter, nameof(chapter));
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            // Update properties only if the corresponding value is not the sentinel value (e.g., 0)
            if (dto.ChapterNumber == 0)  // Assuming 0 is an invalid value for ChapterNumber
            {
                return;
            }
            chapter.ChapterNumber = dto.ChapterNumber;
        }


    }
}
