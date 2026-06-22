namespace PasTech.SmartHome.API.Models;

public class DeviceState
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public Device? Device { get; set; }
    public bool IsOn { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Light
    public double Brightness { get; set; } = 1.0;
    public int ColorR { get; set; } = 255;
    public int ColorG { get; set; } = 255;
    public int ColorB { get; set; } = 255;
    public int ColorTemp { get; set; } = 4000; // Kelvin

    // AC
    public double SetTemperature { get; set; } = 25;
    public double CurrentTemperature { get; set; } = 28;
    public string AcMode { get; set; } = "cool";
    public int FanSpeed { get; set; } = 2;
    public bool SwingMode { get; set; }
    public int TimerMinutes { get; set; }

    // Door
    public bool IsLocked { get; set; } = true;
    public string? LastAccessBy { get; set; }
    public string? AccessMethod { get; set; }

    // Camera
    public bool IsRecording { get; set; }
    public bool IsNightVision { get; set; }
    public bool HasMotion { get; set; }
    public string? StreamUrl { get; set; }
    public string? SnapshotUrl { get; set; }

    // Power plug
    public double PowerWatts { get; set; }
    public double Voltage { get; set; }
    public double Current { get; set; }
}
