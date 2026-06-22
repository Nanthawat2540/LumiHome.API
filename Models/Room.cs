namespace LumiHome.API.Models;

public class Room
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }
    public int Floor { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Device> Devices { get; set; } = [];
}
