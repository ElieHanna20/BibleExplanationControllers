using BibleExplanationControllers.Dtos.BookDtos;
using BibleExplanationControllers.Models.Bible;

namespace BibleExplanationControllers.Mappers
{
    public static class BookMapper
    {
        public static BookDto ToBookDto(this Book bookModel) => new()
        {
            Name = bookModel.Name,
            Id = bookModel.Id
        };

        public static Book ToBookModelFromCreateDto(this CreateBookRequestDto bookDto) => new()
        {
            Name = bookDto.Name,
        };


        public static void UpdateBookFromDto(this Book book, UpdateBookRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(book, nameof(book));
            ArgumentNullException.ThrowIfNull(dto, nameof(dto));

            // Update properties
            book.Name = dto.Name;
        }
    }
}
