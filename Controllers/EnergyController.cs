using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasTech.SmartHome.API.Data;

namespace PasTech.SmartHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnergyController(AppDbContext db) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> Summary([FromQuery] int days = 30)
    {
        var since = DateTime.UtcNow.AddDays(-days);
        var usages = await db.EnergyUsages
            .Where(e => e.PeriodStart >= since)
            .GroupBy(e => e.DeviceId)
            .Select(g => new
            {
                DeviceId = g.Key,
                TotalKwh  = g.Sum(e => e.KiloWattHours),
                TotalCost = g.Sum(e => e.CostBaht),
            })
            .ToListAsync();

        var deviceIds = usages.Select(u => u.DeviceId).ToList();
        var devices = await db.Devices
            .Where(d => deviceIds.Contains(d.Id))
            .Select(d => new { d.Id, d.Name, d.Type })
            .ToDictionaryAsync(d => d.Id);

        return Ok(new
        {
            Period     = $"{days} วัน",
            TotalKwh   = usages.Sum(u => u.TotalKwh),
            TotalCost  = usages.Sum(u => u.TotalCost),
            ByDevice   = usages.Select(u => new
            {
                u.DeviceId,
                DeviceName = devices.TryGetValue(u.DeviceId, out var d) ? d.Name : "Unknown",
                DeviceType = devices.TryGetValue(u.DeviceId, out var d2) ? d2.Type.ToString() : "",
                u.TotalKwh,
                u.TotalCost,
            }).OrderByDescending(u => u.TotalKwh),
        });
    }

    [HttpGet("daily")]
    public async Task<IActionResult> Daily([FromQuery] int days = 7)
    {
        var since = DateTime.UtcNow.AddDays(-days).Date;
        var usages = await db.EnergyUsages
            .Where(e => e.PeriodStart >= since)
            .GroupBy(e => e.PeriodStart.Date)
            .Select(g => new
            {
                Date      = g.Key,
                TotalKwh  = g.Sum(e => e.KiloWattHours),
                TotalCost = g.Sum(e => e.CostBaht),
            })
            .OrderBy(g => g.Date)
            .ToListAsync();
        return Ok(usages);
    }
}
