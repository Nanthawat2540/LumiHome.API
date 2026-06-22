namespace PasTech.SmartHome.API.Models;

public class EnergyUsage
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public Device? Device { get; set; }
    public double KiloWattHours { get; set; }
    public double CostBaht { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}
