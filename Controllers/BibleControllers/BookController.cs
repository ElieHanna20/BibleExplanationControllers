using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.BookDtos;
using BibleExplanationControllers.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.BibleControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CanChangeBooksData")]
    public class BookController(BibleDbContext context) : ControllerBase
    {
        private readonly BibleDbContext _context = context;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var books = await _context.Books.Select(b => b.ToBookDto()).ToListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book.ToBookDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookRequestDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBook = await _context.Books
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync(b => b.Name == bookDto.Name);
            if (existingBook != null)
            {
                return Conflict("A book with this name already exists.");
            }

            var bookModel = bookDto.ToBookModelFromCreateDto();
            _context.Books.Add(bookModel);
            await _context.SaveChangesAsync();

            return StatusCode(201, bookModel); // HTTP 201 Created with the created book object
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBookRequestDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound($"No book found with ID {id}.");
            }

            var duplicateBook = await _context.Books
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(b => b.Name == bookDto.Name && b.Id != id);
            if (duplicateBook != null)
            {
                return Conflict("A book with this name already exists.");
            }

            existingBook.UpdateBookFromDto(bookDto);
            await _context.SaveChangesAsync();

            return Ok(existingBook);
        }
    }
}
