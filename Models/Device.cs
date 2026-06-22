namespace LumiHome.API.Models;

public enum DeviceType { Light, AC, Door, Camera, Sensor }

public class Device
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DeviceType Type { get; set; }
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    public string? IpAddress { get; set; }
    public bool IsOnline { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DeviceState? State { get; set; }
    public ICollection<DeviceLog> Logs { get; set; } = [];
}
