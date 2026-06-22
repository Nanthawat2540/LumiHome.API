namespace PasTech.SmartHome.API.Models;

public class Room
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Icon { get; set; } = "🏠";
    public int Floor { get; set; } = 1;
    public double? FloorPlanX { get; set; }
    public double? FloorPlanY { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Device> Devices { get; set; } = [];
    public ICollection<Sensor> Sensors { get; set; } = [];
}
