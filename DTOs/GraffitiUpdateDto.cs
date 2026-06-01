using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Adicione este using para o IFormFile

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

    // --- Nova propriedade para a Imagem ---
    public IFormFile? Image { get; set; }

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
    [Range(-90, 90, ErrorMessage = "Latitude deve ter um valor entre -90 e 90")]
    public double Lat { get; set; }

    [Required]
    [Range(-180, 180, ErrorMessage = "Longitude deve ter um valor entre -180 e 180")]
    public double Lon { get; set; }
}
