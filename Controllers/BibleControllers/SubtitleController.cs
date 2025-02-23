using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.SubtitleDtos;
using BibleExplanationControllers.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.BibleControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CanChangeBooksData")]
    public class SubtitleController(BibleDbContext context) : Controller
    {
        private readonly BibleDbContext _context = context;

        [HttpGet("chapter/{chapterId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubtitlesByChapter(int chapterId)
        {
            // Validate the chapter exists
            var chapterExists = await _context.Chapters
                                               .AnyAsync(c => c.Id == chapterId);

            if (!chapterExists)
            {
                return NotFound("Chapter not found.");
            }

            // Retrieve subtitles for the chapter
            var subtitles = await _context.Subtitles
                                          .Where(s => s.ChapterId == chapterId)
                                          .Select(s => s.ToSubtitleDto())
                                          .ToListAsync();

            return Ok(subtitles);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var subtitle = await _context.Subtitles.FindAsync(id);

            if (subtitle == null)
            {
                return NotFound();
            }

            return Ok(subtitle.ToSubtitleDto());
        }

        // Create a subtitle for a specific chapter 
        [HttpPost("chapter/{chapterId}")]
        public async Task<IActionResult> Create(int chapterId, [FromBody] CreateSubtitleRequestDto subtitleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate that the chapter exists
            var chapterExists = await _context.Chapters
                                               .AnyAsync(c => c.Id == chapterId);

            if (!chapterExists)
            {
                return NotFound("Chapter not found.");
            }

            // Check if the subtitle already exists in the chapter
            var existingSubtitle = await _context.Subtitles
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(s => s.SubtitleName == subtitleDto.SubtitleName && s.ChapterId == chapterId);

            if (existingSubtitle != null)
            {
                return Conflict("A subtitle with this name already exists in the specified chapter.");
            }

            // Map the DTO to the model and associate it with the chapter
            var subtitleModel = subtitleDto.ToSubtitleModelFromCreateDto(chapterId);

            // Add the new subtitle
            _context.Subtitles.Add(subtitleModel);
            await _context.SaveChangesAsync();

            return StatusCode(201, subtitleModel);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var subtitle = await _context.Subtitles.FindAsync(id);

            if (subtitle == null)
            {
                return NotFound($"Subtitle with ID {id} not found.");
            }

            _context.Subtitles.Remove(subtitle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSubtitleRequestDto subtitleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSubtitle = await _context.Subtitles.FindAsync(id);
            if (existingSubtitle == null)
            {
                return NotFound($"No Subtitle found with ID {id}.");
            }

            var duplicateSubtitle = await _context.Subtitles
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.SubtitleName == subtitleDto.SubtitleName && c.Id != id);

            if (duplicateSubtitle != null)
            {
                return Conflict("A Subtitle with this number already exists.");
            }

            existingSubtitle.UpdateSubtitleFromDto(subtitleDto);
            await _context.SaveChangesAsync();

            return Ok(existingSubtitle);
        }

    }
}
