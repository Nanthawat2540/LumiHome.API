using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasTech.SmartHome.API.Data;
using PasTech.SmartHome.API.Models;

namespace PasTech.SmartHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await db.Rooms
            .Include(r => r.Devices).ThenInclude(d => d.State)
            .OrderBy(r => r.Floor).ThenBy(r => r.Name)
            .ToListAsync();

        var result = rooms.Select(r => new
        {
            r.Id, r.Name, r.Icon, r.Floor,
            ActiveDevices = r.Devices.Count(d => d.State != null && d.State.IsOn),
            TotalDevices  = r.Devices.Count,
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var room = await db.Rooms
            .Include(r => r.Devices).ThenInclude(d => d.State)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null) return NotFound();

        return Ok(new
        {
            room.Id, room.Name, room.Icon, room.Floor,
            Devices = room.Devices.Select(d => new
            {
                d.Id, d.Name, d.Type, d.IsOnline,
                State = d.State == null ? null : new
                {
                    d.State.IsOn, d.State.Brightness, d.State.IsLocked,
                    d.State.SetTemperature, d.State.CurrentTemperature,
                    d.State.AcMode, d.State.FanSpeed,
                    d.State.IsRecording, d.State.HasMotion, d.State.IsNightVision,
                    d.State.UpdatedAt
                }
            })
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(Room room)
    {
        db.Rooms.Add(room);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Room input)
    {
        var room = await db.Rooms.FindAsync(id);
        if (room == null) return NotFound();
        room.Name = input.Name;
        room.Icon = input.Icon;
        room.Floor = input.Floor;
        await db.SaveChangesAsync();
        return Ok(room);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var room = await db.Rooms.FindAsync(id);
        if (room == null) return NotFound();
        db.Rooms.Remove(room);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
