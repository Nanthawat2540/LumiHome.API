namespace PasTech.SmartHome.API.Models;

public enum DeviceType { Light, AC, Door, Camera, Sensor, Switch, Plug, Curtain }
public enum DeviceBrand { Generic, Philips, Xiaomi, Samsung, Panasonic, Daikin, Mitsubishi, Carrier, LG }

public class Device
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DeviceType Type { get; set; }
    public DeviceBrand Brand { get; set; } = DeviceBrand.Generic;
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public int RoomId { get; set; }
    public Room? Room { get; set; }
    public string? IpAddress { get; set; }
    public string? MacAddress { get; set; }
    public string? MqttTopic { get; set; }
    public bool IsOnline { get; set; } = true;
    public DateTime? LastSeenAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DeviceState? State { get; set; }
    public ICollection<DeviceLog> Logs { get; set; } = [];
    public ICollection<EnergyUsage> EnergyUsages { get; set; } = [];
}
