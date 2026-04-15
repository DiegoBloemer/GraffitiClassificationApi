using System.ComponentModel.DataAnnotations;

namespace GraffitiClassificationApi.Api.Models;

public class Gang
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Acronym { get; set; } = string.Empty;

    public string? Origin { get; set; }

    // Navigation property: one gang has many graffitis (1:N)
    public ICollection<Graffiti> Graffitis { get; set; } = new List<Graffiti>();
}
