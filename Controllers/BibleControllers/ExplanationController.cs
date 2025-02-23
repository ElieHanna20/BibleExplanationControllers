using BibleExplanationControllers.Data;
using BibleExplanationControllers.Dtos.ExplanationDtos;
using BibleExplanationControllers.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibleExplanationControllers.Controllers.BibleControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CanChangeBooksData")]
    public class ExplanationController(BibleDbContext context) : Controller
    {
        private readonly BibleDbContext _context = context;

        [HttpGet("subtitle/{subtitleId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetExplanationsBySubtitle(int subtitleId)
        {
            // Validate the subtitle exists
            var subtitleExists = await _context.Subtitles
                                                .AnyAsync(s => s.Id == subtitleId);

            if (!subtitleExists)
            {
                return NotFound("Subtitle not found.");
            }

            // Retrieve explanations for the subtitle
            var explanations = await _context.Explanations
                                             .Where(e => e.SubtitleId == subtitleId)
                                             .Select(e => e.ToExplanationDto())
                                             .ToListAsync();

            return Ok(explanations);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var explanation = await _context.Explanations.FindAsync(id);

            if (explanation == null)
            {
                return NotFound();
            }

            return Ok(explanation.ToExplanationDto());
        }

        [HttpPost("subtitle/{subtitleId}")]
        public async Task<IActionResult> Create(int subtitleId, [FromBody] CreateExplanationRequestDto explanationDto)
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
            var explanationModel = explanationDto.ToExplanationModel(); // Use the correct method name

            // Add the new explanation
            _context.Explanations.Add(explanationModel);
            await _context.SaveChangesAsync();

            return StatusCode(201, explanationModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var explanation = await _context.Explanations.FindAsync(id);

            if (explanation == null)
            {
                return NotFound($"Explanation with ID {id} not found.");
            }

            _context.Explanations.Remove(explanation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateExplanationRequestDto explanationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingExplanation = await _context.Explanations.FindAsync(id);
            if (existingExplanation == null)
            {
                return NotFound($"No Explanation found with ID {id}.");
            }

            existingExplanation.UpdateExplanationFromDto(explanationDto);
            await _context.SaveChangesAsync();

            return Ok(existingExplanation);
        }
    }
}
