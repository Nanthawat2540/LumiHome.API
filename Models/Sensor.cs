namespace PasTech.SmartHome.API.Models;

public enum SensorType { Temperature, Humidity, Motion, Smoke, WaterLeak, Door, PIR, CO2, AirQuality, Light }

public class Sensor
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public SensorType Type { get; set; }
    public int? RoomId { get; set; }
    public Room? Room { get; set; }
    public string? MqttTopic { get; set; }
    public bool IsOnline { get; set; } = true;
    public double? LastValue { get; set; }
    public string? Unit { get; set; }
    public DateTime? LastReadingAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<SensorReading> Readings { get; set; } = [];
}

public class SensorReading
{
    public int Id { get; set; }
    public int SensorId { get; set; }
    public Sensor? Sensor { get; set; }
    public double Value { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}
