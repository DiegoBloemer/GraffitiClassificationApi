namespace GraffitiClassificationApi.Api.DTOs;

public class DashboardSummaryDto
{
    public int TotalGraffitis { get; set; }
    public int TotalGangs { get; set; }
    public string PredominantThreatLevel { get; set; } = string.Empty;
}
