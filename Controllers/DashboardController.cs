using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraffitiClassificationApi.Api.Data;
using GraffitiClassificationApi.Api.DTOs;

namespace GraffitiClassificationApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /api/dashboard/summary
    /// Retorna resumo com totais e nível de ameaça predominante
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var totalGraffitis = await _context.Graffitis.CountAsync();
        var totalGangs = await _context.Gangs.CountAsync();

        var predominantThreatLevel = await _context.Graffitis
            .GroupBy(g => g.ThreatLevel)
            .Select(group => new { ThreatLevel = group.Key, Count = group.Count() })
            .OrderByDescending(x => x.Count)
            .Select(x => x.ThreatLevel)
            .FirstOrDefaultAsync() ?? "N/A";

        return Ok(new DashboardSummaryDto
        {
            TotalGraffitis = totalGraffitis,
            TotalGangs = totalGangs,
            PredominantThreatLevel = predominantThreatLevel
        });
    }

    /// <summary>
    /// GET /api/dashboard/graffitis-by-gang
    /// Retorna contagem de pichações por facção (para gráfico de pizza)
    /// </summary>
    [HttpGet("graffitis-by-gang")]
    public async Task<ActionResult<List<ChartDataDto>>> GetGraffitisByGang()
    {
        var data = await _context.Graffitis
            .Include(g => g.Gang)
            .GroupBy(g => g.Gang!.Acronym)
            .Select(group => new ChartDataDto
            {
                Label = group.Key,
                Value = group.Count()
            })
            .OrderByDescending(x => x.Value)
            .ToListAsync();

        return Ok(data);
    }

    /// <summary>
    /// GET /api/dashboard/graffitis-by-state
    /// Retorna contagem de pichações por estado (para mapa de calor)
    /// </summary>
    [HttpGet("graffitis-by-state")]
    public async Task<ActionResult<List<ChartDataDto>>> GetGraffitisByState()
    {
        var data = await _context.Graffitis
            .Include(g => g.Location)
            .Where(g => g.Location != null)
            .GroupBy(g => g.Location!.State)
            .Select(group => new ChartDataDto
            {
                Label = group.Key,
                Value = group.Count()
            })
            .OrderByDescending(x => x.Value)
            .ToListAsync();

        return Ok(data);
    }

    /// <summary>
    /// GET /api/dashboard/graffitis-by-gang-and-state
    /// Retorna contagem de pichações por estado e facção (para gráfico de barras empilhadas)
    /// </summary>
    [HttpGet("graffitis-by-gang-and-state")]
    public async Task<ActionResult<List<StackedChartDataDto>>> GetGraffitisByGangAndState()
    {
        var data = await _context.Graffitis
            .Include(g => g.Gang)
            .Include(g => g.Location)
            .Where(g => g.Location != null && g.Gang != null)
            .GroupBy(g => new { State = g.Location!.State, GangAcronym = g.Gang!.Acronym })
            .Select(group => new
            {
                group.Key.State,
                group.Key.GangAcronym,
                Count = group.Count()
            })
            .ToListAsync();

        var result = data
            .GroupBy(x => x.State)
            .Select(stateGroup => new StackedChartDataDto
            {
                State = stateGroup.Key,
                GangCounts = stateGroup.ToDictionary(
                    x => x.GangAcronym,
                    x => x.Count
                )
            })
            .OrderBy(x => x.State)
            .ToList();

        return Ok(result);
    }
}
