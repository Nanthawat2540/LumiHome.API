using LumiHome.API.Data;
using LumiHome.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumiHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DevicesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? roomId, [FromQuery] string? type)
    {
        var q = db.Devices.Include(d => d.State).Include(d => d.Room).AsQueryable();
        if (roomId.HasValue) q = q.Where(d => d.RoomId == roomId);
        if (!string.IsNullOrEmpty(type) && Enum.TryParse<DeviceType>(type, true, out var dt))
            q = q.Where(d => d.Type == dt);

        var devices = await q.OrderBy(d => d.RoomId).ThenBy(d => d.Name).ToListAsync();
        return Ok(devices.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var d = await db.Devices.Include(d => d.State).Include(d => d.Room).FirstOrDefaultAsync(d => d.Id == id);
        if (d == null) return NotFound();
        return Ok(ToDto(d));
    }

    [HttpPatch("{id}/state")]
    public async Task<IActionResult> UpdateState(int id, [FromBody] StateUpdateRequest req)
    {
        var device = await db.Devices.Include(d => d.State).FirstOrDefaultAsync(d => d.Id == id);
        if (device == null) return NotFound();

        var state = device.State;
        if (state == null)
        {
            state = new DeviceState { DeviceId = id };
            db.DeviceStates.Add(state);
        }

        if (req.IsOn.HasValue)        state.IsOn = req.IsOn.Value;
        if (req.Brightness.HasValue)   state.Brightness = Math.Clamp(req.Brightness.Value, 0, 1);
        if (req.IsLocked.HasValue)     state.IsLocked = req.IsLocked.Value;
        if (req.SetTemperature.HasValue) state.SetTemperature = Math.Clamp(req.SetTemperature.Value, 16, 30);
        if (req.AcMode != null)        state.AcMode = req.AcMode;
        if (req.FanSpeed.HasValue)     state.FanSpeed = req.FanSpeed.Value;
        if (req.IsRecording.HasValue)  state.IsRecording = req.IsRecording.Value;
        if (req.IsNightVision.HasValue) state.IsNightVision = req.IsNightVision.Value;
        state.UpdatedAt = DateTime.UtcNow;

        // Log the action
        var userId = int.TryParse(User.FindFirst("sub")?.Value, out var uid) ? (int?)uid : null;
        db.DeviceLogs.Add(new DeviceLog
        {
            DeviceId = id,
            Action   = req.IsOn.HasValue ? (req.IsOn.Value ? "turn_on" : "turn_off") : "update_state",
            Detail   = System.Text.Json.JsonSerializer.Serialize(req),
            UserId   = userId,
        });

        await db.SaveChangesAsync();
        return Ok(ToDto(device));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Device device)
    {
        db.Devices.Add(device);
        await db.SaveChangesAsync();
        if (device.State == null)
        {
            db.DeviceStates.Add(new DeviceState { DeviceId = device.Id });
            await db.SaveChangesAsync();
        }
        return CreatedAtAction(nameof(GetById), new { id = device.Id }, ToDto(device));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var device = await db.Devices.FindAsync(id);
        if (device == null) return NotFound();
        db.Devices.Remove(device);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static object ToDto(Device d) => new
    {
        d.Id, d.Name,
        Type     = d.Type.ToString(),
        d.RoomId,
        RoomName = d.Room?.Name,
        d.IsOnline,
        State = d.State == null ? null : new
        {
            d.State.IsOn, d.State.Brightness, d.State.IsLocked,
            d.State.SetTemperature, d.State.CurrentTemperature,
            d.State.AcMode, d.State.FanSpeed,
            d.State.IsRecording, d.State.HasMotion, d.State.IsNightVision,
            d.State.UpdatedAt
        }
    };

    public record StateUpdateRequest(
        bool?   IsOn,
        double? Brightness,
        bool?   IsLocked,
        double? SetTemperature,
        string? AcMode,
        int?    FanSpeed,
        bool?   IsRecording,
        bool?   IsNightVision
    );
}
