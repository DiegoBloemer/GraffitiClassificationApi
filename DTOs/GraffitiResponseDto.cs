namespace GraffitiClassificationApi.Api.DTOs;

/// <summary>
/// Flat response DTO for GraffitiLocation.
/// Has no back-reference to Graffiti, making circular references impossible.
/// </summary>
public class LocationResponseDto
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public double Lat { get; set; }
    public double Lon { get; set; }
}

/// <summary>
/// Flat response DTO for Graffiti.
/// Contains gang data (Name and Acronym only) and location data
/// without any inverse navigation properties — circular reference impossible.
/// </summary>
public class GraffitiResponseDto
{
    public int Id { get; set; }
    public DateTime RegisteredAt { get; set; }
    public string VisualDescription { get; set; } = string.Empty;
    public string ThreatLevel { get; set; } = string.Empty;
    public string? ImagePath { get; set; }

    // Flattened gang data — without the Graffitis collection
    public int GangId { get; set; }
    public string GangName { get; set; } = string.Empty;
    public string GangAcronym { get; set; } = string.Empty;

    // Location without back-reference to the graffiti record
    public LocationResponseDto? Location { get; set; }
}
