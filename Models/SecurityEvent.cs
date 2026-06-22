namespace PasTech.SmartHome.API.Models;

public enum SecurityEventType { MotionDetected, DoorOpened, DoorLocked, AlarmTriggered, Intrusion, FireAlert, FloodAlert, Unknown }

public class SecurityEvent
{
    public int Id { get; set; }
    public SecurityEventType EventType { get; set; }
    public int? DeviceId { get; set; }
    public Device? Device { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAcknowledged { get; set; }
    public int? AcknowledgedByUserId { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
