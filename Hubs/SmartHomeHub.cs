using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PasTech.SmartHome.API.Data;
using Microsoft.EntityFrameworkCore;

namespace PasTech.SmartHome.API.Hubs;

[Authorize]
public class SmartHomeHub(AppDbContext db) : Hub
{
    // Client joins a room group to receive device updates for that room
    public async Task JoinRoom(int roomId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId}");

    public async Task LeaveRoom(int roomId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"room-{roomId}");

    // Client joins global feed
    public async Task JoinDashboard()
        => await Groups.AddToGroupAsync(Context.ConnectionId, "dashboard");

    public async Task LeaveDashboard()
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, "dashboard");

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        // Send current summary on connect
        var summary = await db.Devices
            .Include(d => d.State)
            .Select(d => new { d.Id, d.Name, d.Type, d.IsOnline, d.State!.IsOn })
            .ToListAsync();
        await Clients.Caller.SendAsync("InitialState", summary);
    }
}

// Helper class for broadcasting from controllers/services
public interface ISmartHomeNotifier
{
    Task DeviceStateChanged(int deviceId, object state);
    Task SensorReading(int sensorId, double value, string unit);
    Task SecurityAlert(string eventType, string description, int? deviceId);
    Task DeviceOnlineChanged(int deviceId, bool isOnline);
}

public class SmartHomeNotifier(IHubContext<SmartHomeHub> hub) : ISmartHomeNotifier
{
    public Task DeviceStateChanged(int deviceId, object state)
        => hub.Clients.Group("dashboard").SendAsync("DeviceStateChanged", new { deviceId, state });

    public Task SensorReading(int sensorId, double value, string unit)
        => hub.Clients.Group("dashboard").SendAsync("SensorReading", new { sensorId, value, unit });

    public Task SecurityAlert(string eventType, string description, int? deviceId)
        => hub.Clients.All.SendAsync("SecurityAlert", new { eventType, description, deviceId });

    public Task DeviceOnlineChanged(int deviceId, bool isOnline)
        => hub.Clients.Group("dashboard").SendAsync("DeviceOnlineChanged", new { deviceId, isOnline });
}
