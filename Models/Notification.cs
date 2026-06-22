namespace PasTech.SmartHome.API.Models;

public enum NotificationType { Info, Warning, Alert, Security, Energy, System }
public enum NotificationChannel { App, Email, Line, Telegram }

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public NotificationType Type { get; set; } = NotificationType.Info;
    public NotificationChannel Channel { get; set; } = NotificationChannel.App;
    public bool IsRead { get; set; }
    public string? ActionUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int? RelatedDeviceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
}
