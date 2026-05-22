using System.ComponentModel.DataAnnotations;

namespace GraffitiClassificationApi.Api.DTOs;

/// <summary>
/// DTO for creating a Graffiti record via multipart/form-data.
/// Used instead of the entity directly to support file upload (IFormFile).
/// </summary>
public class GraffitiCreateDto
{
    [Required]
    public string VisualDescription { get; set; } = string.Empty;

    /// <summary>Expected values: Low, Medium, High</summary>
    [Required]
    [RegularExpression("^(Low|Medium|High)$", ErrorMessage = "ThreatLevel must be Low, Medium or High.")]
    public string ThreatLevel { get; set; } = string.Empty;

    [Required]
    public int GangId { get; set; }

    // --- GraffitiLocation fields (sent in the same form) ---

    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string Neighborhood { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public double Lat { get; set; }

    [Required]
    public double Lon { get; set; }

    // Optional image file — IFormFile cannot be serialized as JSON,
    // which is why the endpoint uses [FromForm] instead of [FromBody]
    public IFormFile? Image { get; set; }
}
