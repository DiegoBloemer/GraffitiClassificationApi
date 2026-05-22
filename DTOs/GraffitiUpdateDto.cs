using System.ComponentModel.DataAnnotations;

namespace GraffitiClassificationApi.Api.DTOs;

/// <summary>
/// DTO for updating a Graffiti record via PUT.
/// Only mutable fields are exposed — RegisteredAt and Location are not editable.
/// </summary>
public class GraffitiUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string VisualDescription { get; set; } = string.Empty;

    /// <summary>Expected values: Low, Medium, High</summary>
    [Required]
    [RegularExpression("^(Low|Medium|High)$", ErrorMessage = "ThreatLevel must be Low, Medium or High.")]
    public string ThreatLevel { get; set; } = string.Empty;

    [Required]
    public int GangId { get; set; }
}
