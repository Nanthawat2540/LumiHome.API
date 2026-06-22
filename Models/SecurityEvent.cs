namespace LumiHome.API.Models;

public enum SecurityEventType { MotionDetected, DoorOpened, DoorLocked, AlarmTriggered, Unknown }

public class SecurityEvent
{
    public int Id { get; set; }
    public SecurityEventType EventType { get; set; }
    public int? DeviceId { get; set; }
    public Device? Device { get; set; }
    public string? Description { get; set; }
    public bool IsAcknowledged { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
