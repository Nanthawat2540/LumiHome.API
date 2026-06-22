using LumiHome.API.Data;
using LumiHome.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumiHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LogsController(AppDbContext db) : ControllerBase
{
    [HttpGet("devices")]
    public async Task<IActionResult> GetDeviceLogs([FromQuery] int? deviceId, [FromQuery] int limit = 50)
    {
        var q = db.DeviceLogs.Include(l => l.Device).AsQueryable();
        if (deviceId.HasValue) q = q.Where(l => l.DeviceId == deviceId);
        var logs = await q.OrderByDescending(l => l.CreatedAt).Take(limit).ToListAsync();
        return Ok(logs.Select(l => new
        {
            l.Id, l.DeviceId,
            DeviceName = l.Device?.Name,
            l.Action, l.Detail, l.UserId, l.CreatedAt
        }));
    }

    [HttpGet("security")]
    public async Task<IActionResult> GetSecurityEvents([FromQuery] bool? unackOnly, [FromQuery] int limit = 50)
    {
        var q = db.SecurityEvents.Include(e => e.Device).AsQueryable();
        if (unackOnly == true) q = q.Where(e => !e.IsAcknowledged);
        var events = await q.OrderByDescending(e => e.CreatedAt).Take(limit).ToListAsync();
        return Ok(events.Select(e => new
        {
            e.Id,
            EventType  = e.EventType.ToString(),
            e.DeviceId,
            DeviceName = e.Device?.Name,
            e.Description, e.IsAcknowledged, e.CreatedAt
        }));
    }

    [HttpPatch("security/{id}/ack")]
    public async Task<IActionResult> Acknowledge(int id)
    {
        var ev = await db.SecurityEvents.FindAsync(id);
        if (ev == null) return NotFound();
        ev.IsAcknowledged = true;
        await db.SaveChangesAsync();
        return Ok(new { ev.Id, ev.IsAcknowledged });
    }

    [HttpPost("security")]
    public async Task<IActionResult> CreateEvent(SecurityEvent ev)
    {
        db.SecurityEvents.Add(ev);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSecurityEvents), new { }, new { ev.Id, ev.CreatedAt });
    }
}
