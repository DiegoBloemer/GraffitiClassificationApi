using System.ComponentModel.DataAnnotations;

namespace GraffitiClassificationApi.Api.Models;

public class Graffiti
{
    public int Id { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string VisualDescription { get; set; } = string.Empty;

    /// <summary>Expected values: Low, Medium, High</summary>
    [Required]
    public string ThreatLevel { get; set; } = string.Empty;

    [Required]
    public int GangId { get; set; }

    // Relative path of the image saved in wwwroot (e.g. /images/occurrences/abc.jpg)
    public string? ImagePath { get; set; }

    // Navigation properties
    public Gang? Gang { get; set; }
    public GraffitiLocation? Location { get; set; }
}
