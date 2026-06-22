using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasTech.SmartHome.API.Data;
using PasTech.SmartHome.API.Models;

namespace PasTech.SmartHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SensorsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SensorType? type, [FromQuery] int? roomId)
    {
        var q = db.Sensors.Include(s => s.Room).AsQueryable();
        if (type.HasValue) q = q.Where(s => s.Type == type);
        if (roomId.HasValue) q = q.Where(s => s.RoomId == roomId);
        var list = await q.OrderBy(s => s.RoomId).ThenBy(s => s.Name).ToListAsync();
        return Ok(list.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var s = await db.Sensors.Include(s => s.Room).FirstOrDefaultAsync(s => s.Id == id);
        return s == null ? NotFound() : Ok(ToDto(s));
    }

    [HttpGet("{id}/readings")]
    public async Task<IActionResult> GetReadings(int id, [FromQuery] int hours = 24)
    {
        var since = DateTime.UtcNow.AddHours(-hours);
        var readings = await db.SensorReadings
            .Where(r => r.SensorId == id && r.RecordedAt >= since)
            .OrderBy(r => r.RecordedAt)
            .Select(r => new { r.Value, r.RecordedAt })
            .ToListAsync();
        return Ok(readings);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSensorRequest req)
    {
        var sensor = new Sensor
        {
            Name       = req.Name,
            Type       = req.Type,
            RoomId     = req.RoomId,
            MqttTopic  = req.MqttTopic ?? $"pas/sensors/new/data",
            Unit       = req.Unit,
        };
        db.Sensors.Add(sensor);
        await db.SaveChangesAsync();
        sensor.MqttTopic = $"pas/sensors/{sensor.Id}/data";
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = sensor.Id }, ToDto(sensor));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await db.Sensors.FindAsync(id);
        if (s == null) return NotFound();
        db.Sensors.Remove(s);
        await db.SaveChangesAsync();
        return NoContent();
    }

    public record CreateSensorRequest(string Name, SensorType Type, int? RoomId, string? MqttTopic, string? Unit);

    private static object ToDto(Sensor s) => new
    {
        s.Id, s.Name, Type = s.Type.ToString(), s.RoomId, RoomName = s.Room?.Name,
        s.MqttTopic, s.IsOnline, s.LastValue, s.Unit, s.LastReadingAt, s.CreatedAt,
    };
}
