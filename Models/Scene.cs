namespace PasTech.SmartHome.API.Models;

public class Scene
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Icon { get; set; } = "⭐";
    public string? Color { get; set; }
    public string ActionsJson { get; set; } = "[]"; // JSON array of { deviceId, stateChanges }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastActivatedAt { get; set; }
}
