using System.ComponentModel.DataAnnotations;

namespace GraffitiClassificationApi.Api.DTOs;

/// <summary>
/// DTO for updating a Graffiti record via JSON.
/// Includes location fields to allow updating the address.
/// </summary>
public class GraffitiUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string VisualDescription { get; set; } = string.Empty;

    /// <summary>Expected values: Low, Medium, High</summary>
    [Required]
    public string ThreatLevel { get; set; } = string.Empty;

    [Required]
    public int GangId { get; set; }

    [Required]
    public DateTime RegisteredAt { get; set; }

    // --- Location fields (to update the address) ---

    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string Neighborhood { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double Lat { get; set; }

    [Required]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double Lon { get; set; }
}
