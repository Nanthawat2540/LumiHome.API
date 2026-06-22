using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasTech.SmartHome.API.Data;

namespace PasTech.SmartHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool? unreadOnly, [FromQuery] int page = 1, [FromQuery] int size = 20)
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var q = db.Notifications.Where(n => n.UserId == userId).AsQueryable();
        if (unreadOnly == true) q = q.Where(n => !n.IsRead);
        var total = await q.CountAsync();
        var list  = await q.OrderByDescending(n => n.CreatedAt).Skip((page - 1) * size).Take(size).ToListAsync();
        return Ok(new { total, page, size, items = list });
    }

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkRead(int id)
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var n = await db.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (n == null) return NotFound();
        n.IsRead = true;
        n.ReadAt  = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllRead()
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        await db.Notifications.Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true).SetProperty(n => n.ReadAt, DateTime.UtcNow));
        return Ok();
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> UnreadCount()
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var count  = await db.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
        return Ok(new { count });
    }
}
