namespace GraffitiClassificationApi.Api.DTOs;

public class StackedChartDataDto
{
    public string State { get; set; } = string.Empty;
    public Dictionary<string, int> GangCounts { get; set; } = new();
}
