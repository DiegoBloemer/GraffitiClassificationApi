using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraffitiClassificationApi.Api.Data;
using GraffitiClassificationApi.Api.DTOs;
using GraffitiClassificationApi.Api.Models;
using GraffitiClassificationApi.Api.Services;

namespace GraffitiClassificationApi.Api.Controllers;

/// <summary>Graffiti record management.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GraffitisController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IStorageService _storageService;

    public GraffitisController(AppDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    private static readonly HashSet<string> _allowedImageExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    private const long MaxImageSizeBytes = 10 * 1024 * 1024; // 10 MB

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
            var extension = Path.GetExtension(dto.Image.FileName);

            if (!_allowedImageExtensions.Contains(extension))
                return BadRequest(new { message = $"Extensão da imagem '{extension}' não é permitida. Permitidas: {string.Join(", ", _allowedImageExtensions)}." });

            if (dto.Image.Length > MaxImageSizeBytes)
                return BadRequest(new { message = $"Imagem maior que o tamanho máximo: {MaxImageSizeBytes / (1024 * 1024)} MB." });
            // Upload para MinIO
            imagePath = await _storageService.UploadFileAsync(dto.Image, "occurrences");
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

    /// <summary>Updates an existing graffiti record including location and optional image.</summary>
    /// <param name="id">Graffiti record Id to update.</param>
    /// <param name="dto">Updated data including location fields.</param>
    /// <response code="200">Record updated.</response>
    /// <response code="400">Inconsistent Id, invalid data, or gang not found.</response>
    /// <response code="404">Record not found.</response>
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(GraffitiResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromForm] GraffitiUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "The URL Id does not match the request body Id." });

        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid data.", errors = ModelState });

        // Load existing graffiti with location
        var existing = await _context.Graffitis
            .Include(g => g.Location)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (existing is null)
            return NotFound(new { message = $"Graffiti record with Id {id} not found." });

        // Validate gang exists
        bool gangExists = await _context.Gangs.AnyAsync(g => g.Id == dto.GangId);
        if (!gangExists)
            return BadRequest(new { message = $"Gang with Id {dto.GangId} not found." });

        // Update Graffiti fields
        existing.VisualDescription = dto.VisualDescription;
        existing.ThreatLevel       = dto.ThreatLevel;
        existing.GangId            = dto.GangId;
        if (dto.Image is not null)
        {
            var extension = Path.GetExtension(dto.Image.FileName);

            if (!_allowedImageExtensions.Contains(extension))
                return BadRequest(new { message = $"Extensão da imagem '{extension}' não é permitida. Permitidas: {string.Join(", ", _allowedImageExtensions)}." });

            if (dto.Image.Length > MaxImageSizeBytes)
                return BadRequest(new { message = $"Imagem maior que o tamanho máximo: {MaxImageSizeBytes / (1024 * 1024)} MB." });

            if (!string.IsNullOrEmpty(existing.ImagePath))
            {
                await _storageService.DeleteFileAsync(existing.ImagePath);
            }

            existing.ImagePath = await _storageService.UploadFileAsync(dto.Image, "occurrences");
        }

        // Update Location fields
        if (existing.Location is not null)
        {
            existing.Location.Street       = dto.Street;
            existing.Location.Neighborhood = dto.Neighborhood;
            existing.Location.City         = dto.City;
            existing.Location.State        = dto.State;
            existing.Location.Lat          = dto.Lat;
            existing.Location.Lon          = dto.Lon;
        }
        else
        {
            // If location doesn't exist (shouldn't happen), create it
            existing.Location = new GraffitiLocation
            {
                Street       = dto.Street,
                Neighborhood = dto.Neighborhood,
                City         = dto.City,
                State        = dto.State,
                Lat          = dto.Lat,
                Lon          = dto.Lon,
                GraffitiId   = existing.Id
            };
        }

        await _context.SaveChangesAsync();

        // Reload Gang for response
        await _context.Entry(existing).Reference(g => g.Gang).LoadAsync();

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

        // Excluir imagem do MinIO se existir
        if (!string.IsNullOrEmpty(graffiti.ImagePath))
        {
            await _storageService.DeleteFileAsync(graffiti.ImagePath);
        }

        // Removing the record also removes the dependent Location (EF Core cascade)
        _context.Graffitis.Remove(graffiti);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
