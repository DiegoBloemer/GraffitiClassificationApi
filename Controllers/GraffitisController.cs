using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraffitiClassificationApi.Api.Data;
using GraffitiClassificationApi.Api.DTOs;
using GraffitiClassificationApi.Api.Models;

namespace GraffitiClassificationApi.Api.Controllers;

/// <summary>Graffiti record management.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GraffitisController : ControllerBase
{
    private readonly AppDbContext _context;

    public GraffitisController(AppDbContext context)
    {
        _context = context;
    }

    // Maps a Graffiti entity to a flat response DTO, breaking any circular reference.
    private static GraffitiResponseDto ToDto(Graffiti g) => new()
    {
        Id               = g.Id,
        RegisteredAt     = g.RegisteredAt,
        VisualDescription = g.VisualDescription,
        ThreatLevel      = g.ThreatLevel,
        ImagePath        = g.ImagePath,
        GangId           = g.GangId,
        GangName         = g.Gang?.Name ?? string.Empty,
        GangAcronym      = g.Gang?.Acronym ?? string.Empty,
        Location         = g.Location is null ? null : new LocationResponseDto
        {
            Id           = g.Location.Id,
            Street       = g.Location.Street,
            Neighborhood = g.Location.Neighborhood,
            City         = g.Location.City,
            State        = g.Location.State,
            Lat          = g.Location.Lat,
            Lon          = g.Location.Lon
        }
    };

    /// <summary>Returns all graffiti records with gang and location included.</summary>
    /// <response code="200">List of graffiti records.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GraffitiResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var graffitis = await _context.Graffitis
            .Include(g => g.Gang)
            .Include(g => g.Location)
            .ToListAsync();

        return Ok(graffitis.Select(ToDto));
    }

    /// <summary>Returns a graffiti record by Id with gang and location included.</summary>
    /// <param name="id">Graffiti record Id.</param>
    /// <response code="200">Record found.</response>
    /// <response code="404">Record not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GraffitiResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var graffiti = await _context.Graffitis
            .Include(g => g.Gang)
            .Include(g => g.Location)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (graffiti is null)
            return NotFound(new { message = $"Graffiti record with Id {id} not found." });

        return Ok(ToDto(graffiti));
    }

    /// <summary>
    /// Creates a new graffiti record via multipart/form-data.
    /// Send location fields in the form and optionally an image file.
    /// </summary>
    /// <response code="201">Record created successfully.</response>
    /// <response code="400">Invalid data or gang not found.</response>
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(GraffitiResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm] GraffitiCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid data.", errors = ModelState });

        var gang = await _context.Gangs.FindAsync(dto.GangId);
        if (gang is null)
            return BadRequest(new { message = $"Gang with Id {dto.GangId} not found." });

        string? imagePath = null;

        if (dto.Image is not null)
        {
            // Build the absolute physical path to the destination folder inside wwwroot
            var destFolder = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "images", "occurrences");

            // Create the full directory hierarchy if it does not exist
            Directory.CreateDirectory(destFolder);

            // Preserve the original file extension (e.g. .jpg, .png)
            var extension = Path.GetExtension(dto.Image.FileName);

            // Generate a unique file name to avoid collisions
            var fileName    = $"{Guid.NewGuid()}{extension}";
            var physicalPath = Path.Combine(destFolder, fileName);

            // Copy the IFormFile content to disk using a FileStream
            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            // Relative path saved in the database and used by the front-end
            imagePath = $"/images/occurrences/{fileName}";
        }

        var graffiti = new Graffiti
        {
            VisualDescription = dto.VisualDescription,
            ThreatLevel       = dto.ThreatLevel,
            GangId            = dto.GangId,
            ImagePath         = imagePath,
            Location          = new GraffitiLocation
            {
                Street       = dto.Street,
                Neighborhood = dto.Neighborhood,
                City         = dto.City,
                State        = dto.State,
                Lat          = dto.Lat,
                Lon          = dto.Lon
            }
        };

        _context.Graffitis.Add(graffiti);
        await _context.SaveChangesAsync();

        // Assign the already-loaded Gang entity so ToDto avoids an extra round-trip
        graffiti.Gang = gang;

        return CreatedAtAction(nameof(GetById), new { id = graffiti.Id }, ToDto(graffiti));
    }

    /// <summary>Updates an existing graffiti record (does not update location).</summary>
    /// <param name="id">Graffiti record Id to update.</param>
    /// <response code="200">Record updated.</response>
    /// <response code="400">Inconsistent Id, invalid data, or gang not found.</response>
    /// <response code="404">Record not found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GraffitiResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Graffiti updated)
    {
        if (id != updated.Id)
            return BadRequest(new { message = "The URL Id does not match the request body Id." });

        var existing = await _context.Graffitis.FindAsync(id);
        if (existing is null)
            return NotFound(new { message = $"Graffiti record with Id {id} not found." });

        bool gangExists = await _context.Gangs.AnyAsync(g => g.Id == updated.GangId);
        if (!gangExists)
            return BadRequest(new { message = $"Gang with Id {updated.GangId} not found." });

        existing.VisualDescription = updated.VisualDescription;
        existing.ThreatLevel       = updated.ThreatLevel;
        existing.GangId            = updated.GangId;
        existing.RegisteredAt      = updated.RegisteredAt;

        await _context.SaveChangesAsync();

        // Reload related entities to build the full response DTO
        await _context.Entry(existing).Reference(g => g.Gang).LoadAsync();
        await _context.Entry(existing).Reference(g => g.Location).LoadAsync();

        return Ok(ToDto(existing));
    }

    /// <summary>Deletes a graffiti record and its associated location.</summary>
    /// <param name="id">Graffiti record Id to delete.</param>
    /// <response code="204">Record deleted successfully.</response>
    /// <response code="404">Record not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var graffiti = await _context.Graffitis
            .Include(g => g.Location)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (graffiti is null)
            return NotFound(new { message = $"Graffiti record with Id {id} not found." });

        // Removing the record also removes the dependent Location (EF Core cascade)
        _context.Graffitis.Remove(graffiti);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
