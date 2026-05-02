using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraffitiClassificationApi.Api.Data;
using GraffitiClassificationApi.Api.Models;

namespace GraffitiClassificationApi.Api.Controllers;

/// <summary>Criminal gang management.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GangsController : ControllerBase
{
    private readonly AppDbContext _context;

    public GangsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Returns all registered gangs.</summary>
    /// <response code="200">List of gangs.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Gang>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var gangs = await _context.Gangs.ToListAsync();
        return Ok(gangs);
    }

    /// <summary>Returns a gang by Id.</summary>
    /// <param name="id">Gang Id.</param>
    /// <response code="200">Gang found.</response>
    /// <response code="404">Gang not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Gang), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var gang = await _context.Gangs.FindAsync(id);
        if (gang is null)
            return NotFound(new { message = $"Gang with Id {id} not found." });

        return Ok(gang);
    }

    /// <summary>Creates a new gang.</summary>
    /// <response code="201">Gang created successfully.</response>
    /// <response code="400">Invalid data.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Gang), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] Gang gang)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid data.", errors = ModelState });

        _context.Gangs.Add(gang);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = gang.Id }, gang);
    }

    /// <summary>Updates an existing gang.</summary>
    /// <param name="id">Gang Id to update.</param>
    /// <response code="200">Gang updated.</response>
    /// <response code="400">Inconsistent Id or invalid data.</response>
    /// <response code="404">Gang not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Gang), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Gang updatedGang)
    {
        if (id != updatedGang.Id)
            return BadRequest(new { message = "The URL Id does not match the request body Id." });

        var existing = await _context.Gangs.FindAsync(id);
        if (existing is null)
            return NotFound(new { message = $"Gang with Id {id} not found." });

        existing.Name    = updatedGang.Name;
        existing.Acronym = updatedGang.Acronym;
        existing.Origin  = updatedGang.Origin;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    /// <summary>Deletes a gang. Blocked if it has linked graffiti records.</summary>
    /// <param name="id">Gang Id to delete.</param>
    /// <response code="204">Gang deleted successfully.</response>
    /// <response code="404">Gang not found.</response>
    /// <response code="409">Gang has linked graffiti records.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        var gang = await _context.Gangs.FindAsync(id);
        if (gang is null)
            return NotFound(new { message = $"Gang with Id {id} not found." });

        bool hasGraffitis = await _context.Graffitis.AnyAsync(g => g.GangId == id);
        if (hasGraffitis)
            return Conflict(new { message = "Cannot delete the gang because it has linked graffiti records." });

        _context.Gangs.Remove(gang);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
