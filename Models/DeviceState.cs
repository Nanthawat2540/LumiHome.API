namespace LumiHome.API.Models;

public class DeviceState
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public Device? Device { get; set; }

    // Shared
    public bool IsOn { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Light
    public double Brightness { get; set; } = 1.0;

    // AC
    public double SetTemperature { get; set; } = 25;
    public double CurrentTemperature { get; set; } = 28;
    public string AcMode { get; set; } = "cool";
    public int FanSpeed { get; set; } = 2;

    // Door
    public bool IsLocked { get; set; } = true;

    // Camera
    public bool IsRecording { get; set; }
    public bool IsNightVision { get; set; }
    public bool HasMotion { get; set; }
}
