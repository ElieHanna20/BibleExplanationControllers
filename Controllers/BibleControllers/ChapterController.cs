using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.ChapterDtos;
using BibleExplanationControllers.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BibleExplanationControllers.Controllers.BibleControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CanChangeBooksData")]
    public class ChapterController(BibleDbContext context) : ControllerBase
    {
        private readonly BibleDbContext _context = context;

        [HttpGet("book/{bookId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChaptersByBook(int bookId)
        {
            // Check if the book exists
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            // Retrieve chapters for the specified book
            var chapters = await _context.Chapters
                                          .Where(c => c.BookId == bookId)
                                          .Select(c => c.ToChapterDto())
                                          .ToListAsync();

            return Ok(chapters);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChapterById(int id)
        {

            var chapter = await _context.Chapters.FindAsync(id);

            if (chapter == null)
            {
                return NotFound($"Chapter with ID : {id} not found.");
            }

            return Ok(chapter.ToChapterDto());
        }

        [HttpPost("book/{bookId}")]
        public async Task<IActionResult> Create(int bookId, [FromBody] CreateChapterRequestDto chapterDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var book = await _context.Books.FindAsync(bookId);

            if (book == null) { return NotFound("Book not found."); } // Check if the chapter already exists in the book

            var existingChapter = await _context.Chapters.AsNoTracking()
                                                         .FirstOrDefaultAsync(c => c.ChapterNumber == chapterDto.ChapterNumber && c.BookId == bookId);

            if (existingChapter != null)
            {
                return Conflict("A chapter with this number already exists in the specified book.");
            } // Map the DTO to the model

            var chapterModel = chapterDto.ToChapterModelFromCreateDto(bookId);
            // Add the new chapter to the context
            _context.Chapters.Add(chapterModel);

            await _context.SaveChangesAsync();

            return StatusCode(201, chapterModel);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);

            if (chapter == null)
            {
                return NotFound($"Chapter with ID {id} not found.");
            }

            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateChapterRequestDto chapterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingChapter = await _context.Chapters.FindAsync(id);
            if (existingChapter == null)
            {
                return NotFound($"No Chapter found with ID {id}.");
            }

            var duplicateChapter = await _context.Chapters
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ChapterNumber == chapterDto.ChapterNumber && c.Id != id);

            if (duplicateChapter != null)
            {
                return Conflict("A Chapter with this number already exists.");
            }

            existingChapter.UpdateChapterFromDto(chapterDto);
            await _context.SaveChangesAsync();

            return Ok(existingChapter); // Return the updated Chapter
        }


    }
}
