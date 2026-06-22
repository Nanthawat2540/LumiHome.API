namespace PasTech.SmartHome.API.Models;

public class DeviceLog
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public Device? Device { get; set; }
    public required string Action { get; set; }
    public string? Detail { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public string? Source { get; set; } // app, automation, mqtt, schedule
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
