using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.VerseDtos;
using BibleExplanationControllers.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.BibleControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CanChangeBooksData")]
    public class VerseController(BibleDbContext context) : Controller
    {
        private readonly BibleDbContext _context = context;

        [HttpGet("subtitle/{subtitleId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVersesBySubtitle(int subtitleId)
        {
            // Validate the subtitle exists
            var subtitleExists = await _context.Subtitles
                                                .AnyAsync(s => s.Id == subtitleId);

            if (!subtitleExists)
            {
                return NotFound("Subtitle not found.");
            }

            // Retrieve verses for the subtitle
            var verses = await _context.Verses
                                       .Where(v => v.SubtitleId == subtitleId)
                                       .Select(v => v.ToVerseDto())
                                       .ToListAsync();

            return Ok(verses);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var verse = await _context.Verses.FindAsync(id);

            if (verse == null)
            {
                return NotFound();
            }

            return Ok(verse.ToVerseDto());
        }

        // Create a verse for a specific chapter 
        [HttpPost("subtitle/{subtitleId}")]
        public async Task<IActionResult> Create(int subtitleId, [FromBody] CreateVerseRequestDto verseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate that the subtitle exists
            var subtitleExists = await _context.Subtitles
                                               .AnyAsync(c => c.Id == subtitleId);

            if (!subtitleExists)
            {
                return NotFound("Subtitle not found.");
            }

            // Map the DTO to the model and associate it with the subtitle
            var verseModel = verseDto.ToSubtitleModelFromCreateDto(subtitleId);

            // Add the new subtitle
            _context.Verses.Add(verseModel);
            await _context.SaveChangesAsync();

            return StatusCode(201, verseModel);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var verse = await _context.Verses.FindAsync(id);

            if (verse == null)
            {
                return NotFound($"Verse with ID {id} not found.");
            }

            _context.Verses.Remove(verse);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateVerseRequestDto verseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingVerseDto = await _context.Verses.FindAsync(id);
            if (existingVerseDto == null)
            {
                return NotFound($"No Verse found with ID {id}.");
            }

            existingVerseDto.UpdateVerseDtoFromDto(verseDto);
            await _context.SaveChangesAsync();

            return Ok(existingVerseDto);
        }

    }
}
