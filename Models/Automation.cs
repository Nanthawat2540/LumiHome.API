namespace PasTech.SmartHome.API.Models;

public enum TriggerType { DeviceState, SensorThreshold, Schedule, Sunrise, Sunset, Manual }
public enum ConditionOperator { GreaterThan, LessThan, Equal, NotEqual, Between }
public enum ActionType { TurnOn, TurnOff, SetBrightness, SetTemperature, Lock, Unlock, SendNotification, Delay, RunScene }

public class Automation
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; } = true;
    public TriggerType TriggerType { get; set; }
    public string? TriggerConfig { get; set; } // JSON
    public string? ConditionConfig { get; set; } // JSON
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastTriggeredAt { get; set; }
    public int TriggerCount { get; set; }
    public ICollection<AutomationAction> Actions { get; set; } = [];
}

public class AutomationAction
{
    public int Id { get; set; }
    public int AutomationId { get; set; }
    public Automation? Automation { get; set; }
    public int Order { get; set; }
    public ActionType ActionType { get; set; }
    public int? TargetDeviceId { get; set; }
    public Device? TargetDevice { get; set; }
    public string? ActionConfig { get; set; } // JSON: { "value": 24, "mode": "cool" }
    public int DelaySeconds { get; set; }
}
