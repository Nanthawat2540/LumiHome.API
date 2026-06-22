using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasTech.SmartHome.API.Data;
using PasTech.SmartHome.API.Models;

namespace PasTech.SmartHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AutomationController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await db.Automations.Include(a => a.Actions).OrderByDescending(a => a.CreatedAt).ToListAsync();
        return Ok(list.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var a = await db.Automations.Include(a => a.Actions).ThenInclude(x => x.TargetDevice).FirstOrDefaultAsync(a => a.Id == id);
        return a == null ? NotFound() : Ok(ToDto(a));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAutomationRequest req)
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var automation = new Automation
        {
            Name            = req.Name,
            Description     = req.Description,
            TriggerType     = req.TriggerType,
            TriggerConfig   = req.TriggerConfig,
            ConditionConfig = req.ConditionConfig,
            CreatedByUserId = userId,
        };
        foreach (var (act, i) in req.Actions.Select((a, i) => (a, i)))
        {
            automation.Actions.Add(new AutomationAction
            {
                Order        = i,
                ActionType   = act.ActionType,
                TargetDeviceId = act.TargetDeviceId,
                ActionConfig = act.ActionConfig,
                DelaySeconds = act.DelaySeconds,
            });
        }
        db.Automations.Add(automation);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = automation.Id }, ToDto(automation));
    }

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> Toggle(int id)
    {
        var a = await db.Automations.FindAsync(id);
        if (a == null) return NotFound();
        a.IsEnabled = !a.IsEnabled;
        await db.SaveChangesAsync();
        return Ok(new { a.Id, a.IsEnabled });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var a = await db.Automations.FindAsync(id);
        if (a == null) return NotFound();
        db.Automations.Remove(a);
        await db.SaveChangesAsync();
        return NoContent();
    }

    public record ActionRequest(ActionType ActionType, int? TargetDeviceId, string? ActionConfig, int DelaySeconds = 0);
    public record CreateAutomationRequest(string Name, string? Description, TriggerType TriggerType,
        string? TriggerConfig, string? ConditionConfig, List<ActionRequest> Actions);

    private static object ToDto(Automation a) => new
    {
        a.Id, a.Name, a.Description, a.IsEnabled, TriggerType = a.TriggerType.ToString(),
        a.TriggerConfig, a.ConditionConfig, a.CreatedAt, a.LastTriggeredAt, a.TriggerCount,
        Actions = a.Actions.OrderBy(x => x.Order).Select(x => new {
            x.Id, x.Order, ActionType = x.ActionType.ToString(),
            x.TargetDeviceId, TargetDevice = x.TargetDevice?.Name,
            x.ActionConfig, x.DelaySeconds,
        }),
    };
}
