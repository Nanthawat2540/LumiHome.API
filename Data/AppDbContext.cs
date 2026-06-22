using Microsoft.EntityFrameworkCore;
using PasTech.SmartHome.API.Models;

namespace PasTech.SmartHome.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceState> DeviceStates => Set<DeviceState>();
    public DbSet<DeviceLog> DeviceLogs => Set<DeviceLog>();
    public DbSet<SecurityEvent> SecurityEvents => Set<SecurityEvent>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<SensorReading> SensorReadings => Set<SensorReading>();
    public DbSet<Automation> Automations => Set<Automation>();
    public DbSet<AutomationAction> AutomationActions => Set<AutomationAction>();
    public DbSet<Scene> Scenes => Set<Scene>();
    public DbSet<EnergyUsage> EnergyUsages => Set<EnergyUsage>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Device>().HasOne(d => d.State)
            .WithOne(s => s.Device)
            .HasForeignKey<DeviceState>(s => s.DeviceId);

        mb.Entity<DeviceLog>().HasOne(l => l.User)
            .WithMany(u => u.DeviceLogs)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        mb.Entity<RefreshToken>().HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Notification>().HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<EnergyUsage>().HasOne(e => e.Device)
            .WithMany(d => d.EnergyUsages)
            .HasForeignKey(e => e.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<AutomationAction>().HasOne(a => a.TargetDevice)
            .WithMany().HasForeignKey(a => a.TargetDeviceId)
            .OnDelete(DeleteBehavior.SetNull);

        mb.Entity<Device>().Property(d => d.Type).HasConversion<string>();
        mb.Entity<Device>().Property(d => d.Brand).HasConversion<string>();
        mb.Entity<SecurityEvent>().Property(e => e.EventType).HasConversion<string>();
        mb.Entity<User>().Property(u => u.Role).HasConversion<string>();
        mb.Entity<Sensor>().Property(s => s.Type).HasConversion<string>();
        mb.Entity<Automation>().Property(a => a.TriggerType).HasConversion<string>();
        mb.Entity<AutomationAction>().Property(a => a.ActionType).HasConversion<string>();
        mb.Entity<Notification>().Property(n => n.Type).HasConversion<string>();
        mb.Entity<Notification>().Property(n => n.Channel).HasConversion<string>();

        // Seed Rooms
        mb.Entity<Room>().HasData(
            new Room { Id = 1, Name = "ห้องนั่งเล่น",  Icon = "🛋️", Floor = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 2, Name = "ห้องนอนหลัก",   Icon = "🛏️", Floor = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 3, Name = "ห้องครัว",       Icon = "🍳", Floor = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 4, Name = "ห้องน้ำ",        Icon = "🚿", Floor = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 5, Name = "ห้องทำงาน",      Icon = "💼", Floor = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 6, Name = "ห้องนอนลูก",     Icon = "🧸", Floor = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 7, Name = "โรงรถ",          Icon = "🚗", Floor = 0, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Room { Id = 8, Name = "สวน",            Icon = "🌿", Floor = 0, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        mb.Entity<Device>().HasData(
            new Device { Id = 1,  Name = "ไฟห้องนั่งเล่น",    Type = DeviceType.Light,  RoomId = 1, MqttTopic = "pas/devices/1/state",  IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 2,  Name = "ไฟห้องนอน",          Type = DeviceType.Light,  RoomId = 2, MqttTopic = "pas/devices/2/state",  IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 3,  Name = "ไฟห้องครัว",         Type = DeviceType.Light,  RoomId = 3, MqttTopic = "pas/devices/3/state",  IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 4,  Name = "ไฟห้องทำงาน",        Type = DeviceType.Light,  RoomId = 5, MqttTopic = "pas/devices/4/state",  IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 5,  Name = "แอร์ห้องนั่งเล่น",  Type = DeviceType.AC, Brand = DeviceBrand.Daikin,     RoomId = 1, MqttTopic = "pas/devices/5/state",  IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 6,  Name = "แอร์ห้องนอน",        Type = DeviceType.AC, Brand = DeviceBrand.Mitsubishi, RoomId = 2, MqttTopic = "pas/devices/6/state",  IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 7,  Name = "แอร์ห้องทำงาน",      Type = DeviceType.AC, Brand = DeviceBrand.LG,         RoomId = 5, MqttTopic = "pas/devices/7/state",  IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 8,  Name = "ประตูหน้าบ้าน",      Type = DeviceType.Door,   RoomId = 1, MqttTopic = "pas/devices/8/state",  IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 9,  Name = "ประตูโรงรถ",          Type = DeviceType.Door,   RoomId = 7, MqttTopic = "pas/devices/9/state",  IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 10, Name = "ประตูหลัง",           Type = DeviceType.Door,   RoomId = 8, MqttTopic = "pas/devices/10/state", IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 11, Name = "กล้องหน้าบ้าน",       Type = DeviceType.Camera, RoomId = 1, MqttTopic = "pas/devices/11/state", IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 12, Name = "กล้องสวนหลัง",         Type = DeviceType.Camera, RoomId = 8, MqttTopic = "pas/devices/12/state", IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 13, Name = "กล้องโรงรถ",           Type = DeviceType.Camera, RoomId = 7, MqttTopic = "pas/devices/13/state", IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Device { Id = 14, Name = "กล้องห้องนั่งเล่น",    Type = DeviceType.Camera, RoomId = 1, MqttTopic = "pas/devices/14/state", IsOnline = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        mb.Entity<DeviceState>().HasData(
            new DeviceState { Id = 1,  DeviceId = 1,  IsOn = true,  Brightness = 0.8, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 2,  DeviceId = 2,  IsOn = false, Brightness = 1.0, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 3,  DeviceId = 3,  IsOn = true,  Brightness = 0.6, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 4,  DeviceId = 4,  IsOn = true,  Brightness = 0.9, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 5,  DeviceId = 5,  IsOn = true,  SetTemperature = 24, CurrentTemperature = 26, AcMode = "cool", FanSpeed = 2, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 6,  DeviceId = 6,  IsOn = false, SetTemperature = 25, CurrentTemperature = 28, AcMode = "cool", FanSpeed = 1, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 7,  DeviceId = 7,  IsOn = true,  SetTemperature = 23, CurrentTemperature = 27, AcMode = "cool", FanSpeed = 3, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 8,  DeviceId = 8,  IsLocked = true,  UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 9,  DeviceId = 9,  IsLocked = false, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 10, DeviceId = 10, IsLocked = true,  UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 11, DeviceId = 11, IsRecording = true, IsNightVision = false, HasMotion = true,  UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 12, DeviceId = 12, IsRecording = true, IsNightVision = true,  HasMotion = false, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 13, DeviceId = 13, IsRecording = true, IsNightVision = false, HasMotion = false, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new DeviceState { Id = 14, DeviceId = 14, IsRecording = true, IsNightVision = true,  HasMotion = false, UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        mb.Entity<Sensor>().HasData(
            new Sensor { Id = 1, Name = "อุณหภูมิห้องนั่งเล่น",  Type = SensorType.Temperature, RoomId = 1, Unit = "°C",  LastValue = 26.5, MqttTopic = "pas/sensors/1", LastReadingAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Sensor { Id = 2, Name = "ความชื้นห้องนั่งเล่น",   Type = SensorType.Humidity,    RoomId = 1, Unit = "%",   LastValue = 62.0, MqttTopic = "pas/sensors/2", LastReadingAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Sensor { Id = 3, Name = "เซนเซอร์การเคลื่อนไหว",  Type = SensorType.Motion,      RoomId = 1, Unit = "",    LastValue = 0,    MqttTopic = "pas/sensors/3", LastReadingAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Sensor { Id = 4, Name = "เซนเซอร์ควัน",           Type = SensorType.Smoke,       RoomId = 3, Unit = "ppm", LastValue = 0,    MqttTopic = "pas/sensors/4", LastReadingAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Sensor { Id = 5, Name = "เซนเซอร์น้ำรั่ว",        Type = SensorType.WaterLeak,   RoomId = 4, Unit = "",    LastValue = 0,    MqttTopic = "pas/sensors/5", LastReadingAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
