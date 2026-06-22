using System.Text;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using PasTech.SmartHome.API.Data;
using PasTech.SmartHome.API.Hubs;
using PasTech.SmartHome.API.Models;
using Microsoft.EntityFrameworkCore;

namespace PasTech.SmartHome.API.Services;

public class MqttService : IHostedService, IDisposable
{
    private IManagedMqttClient? _client;
    private readonly IConfiguration _config;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ISmartHomeNotifier _notifier;
    private readonly ILogger<MqttService> _logger;

    public MqttService(IConfiguration config, IServiceScopeFactory scopeFactory,
        ISmartHomeNotifier notifier, ILogger<MqttService> logger)
    {
        _config = config;
        _scopeFactory = scopeFactory;
        _notifier = notifier;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var host     = _config["Mqtt:Host"] ?? "localhost";
        var port     = int.Parse(_config["Mqtt:Port"] ?? "1883");
        var user     = _config["Mqtt:Username"];
        var password = _config["Mqtt:Password"];

        var options = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(new MqttClientOptionsBuilder()
                .WithTcpServer(host, port)
                .WithClientId($"PasSmartHome-API-{Guid.NewGuid():N}")
                .WithCredentials(user, password)
                .WithCleanSession()
                .Build())
            .Build();

        _client = new MqttFactory().CreateManagedMqttClient();
        _client.ApplicationMessageReceivedAsync += OnMessageReceived;

        await _client.StartAsync(options);
        await _client.SubscribeAsync("pas/devices/+/telemetry");
        await _client.SubscribeAsync("pas/sensors/+/data");
        await _client.SubscribeAsync("pas/devices/+/status");
        _logger.LogInformation("MQTT connected to {Host}:{Port}", host, port);
    }

    private async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
    {
        var topic   = e.ApplicationMessage.Topic;
        var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

        try
        {
            var parts = topic.Split('/');
            if (parts.Length < 4) return;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (parts[1] == "devices" && int.TryParse(parts[2], out int deviceId))
            {
                if (parts[3] == "telemetry")
                    await HandleDeviceTelemetry(db, deviceId, payload);
                else if (parts[3] == "status")
                    await HandleDeviceStatus(db, deviceId, payload);
            }
            else if (parts[1] == "sensors" && int.TryParse(parts[2], out int sensorId))
            {
                await HandleSensorData(db, sensorId, payload);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MQTT message error on topic {Topic}", topic);
        }
    }

    private async Task HandleDeviceTelemetry(AppDbContext db, int deviceId, string payload)
    {
        var doc = JsonDocument.Parse(payload);
        var state = await db.DeviceStates.FirstOrDefaultAsync(s => s.DeviceId == deviceId);
        if (state == null) return;

        if (doc.RootElement.TryGetProperty("isOn", out var isOn))
            state.IsOn = isOn.GetBoolean();
        if (doc.RootElement.TryGetProperty("brightness", out var brightness))
            state.Brightness = brightness.GetDouble();
        if (doc.RootElement.TryGetProperty("temperature", out var temp))
            state.CurrentTemperature = temp.GetDouble();
        if (doc.RootElement.TryGetProperty("isLocked", out var locked))
            state.IsLocked = locked.GetBoolean();
        if (doc.RootElement.TryGetProperty("hasMotion", out var motion))
            state.HasMotion = motion.GetBoolean();

        state.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        await _notifier.DeviceStateChanged(deviceId, new { state.IsOn, state.Brightness, state.IsLocked, state.HasMotion });
    }

    private async Task HandleDeviceStatus(AppDbContext db, int deviceId, string payload)
    {
        var device = await db.Devices.FindAsync(deviceId);
        if (device == null) return;
        device.IsOnline  = payload.Trim('"') == "online";
        device.LastSeenAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        await _notifier.DeviceOnlineChanged(deviceId, device.IsOnline);
    }

    private async Task HandleSensorData(AppDbContext db, int sensorId, string payload)
    {
        var doc    = JsonDocument.Parse(payload);
        var value  = doc.RootElement.GetProperty("value").GetDouble();
        var sensor = await db.Sensors.FindAsync(sensorId);
        if (sensor == null) return;

        sensor.LastValue      = value;
        sensor.LastReadingAt  = DateTime.UtcNow;
        db.SensorReadings.Add(new SensorReading { SensorId = sensorId, Value = value });
        await db.SaveChangesAsync();
        await _notifier.SensorReading(sensorId, value, sensor.Unit ?? "");
    }

    public async Task PublishCommand(string topic, object payload)
    {
        if (_client == null) return;
        var json = JsonSerializer.Serialize(payload);
        var msg  = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
            .WithRetainFlag()
            .Build();
        await _client.EnqueueAsync(msg);
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => _client?.StopAsync() ?? Task.CompletedTask;

    public void Dispose() => _client?.Dispose();
}
