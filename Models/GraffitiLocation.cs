using System.ComponentModel.DataAnnotations;

namespace GraffitiClassificationApi.Api.Models;

public class GraffitiLocation
{
    public int Id { get; set; }

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

    // Mandatory and unique FK — enforces the 1:1 relationship with Graffiti
    [Required]
    public int GraffitiId { get; set; }

    // Navigation property back to the graffiti record
    public Graffiti? Graffiti { get; set; }
}
