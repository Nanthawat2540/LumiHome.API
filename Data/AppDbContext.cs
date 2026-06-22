using LumiHome.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LumiHome.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceState> DeviceStates { get; set; }
    public DbSet<DeviceLog> DeviceLogs { get; set; }
    public DbSet<SecurityEvent> SecurityEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Device>()
            .HasOne(d => d.State)
            .WithOne(s => s.Device)
            .HasForeignKey<DeviceState>(s => s.DeviceId);

        mb.Entity<Device>()
            .HasOne(d => d.Room)
            .WithMany(r => r.Devices)
            .HasForeignKey(d => d.RoomId);

        mb.Entity<Device>()
            .Property(d => d.Type)
            .HasConversion<string>();

        mb.Entity<SecurityEvent>()
            .Property(e => e.EventType)
            .HasConversion<string>();

        // Seed rooms
        mb.Entity<Room>().HasData(
            new Room { Id = 1, Name = "ห้องนั่งเล่น", Icon = "🛋️", Floor = 1 },
            new Room { Id = 2, Name = "ห้องนอนหลัก", Icon = "🛏️", Floor = 2 },
            new Room { Id = 3, Name = "ห้องครัว",    Icon = "🍳", Floor = 1 },
            new Room { Id = 4, Name = "ห้องน้ำ",     Icon = "🚿", Floor = 1 },
            new Room { Id = 5, Name = "ห้องทำงาน",   Icon = "💻", Floor = 2 },
            new Room { Id = 6, Name = "ห้องนอนลูก",  Icon = "🧸", Floor = 2 },
            new Room { Id = 7, Name = "โรงรถ",       Icon = "🚗", Floor = 0 },
            new Room { Id = 8, Name = "สวน",         Icon = "🌿", Floor = 0 }
        );

        // Seed devices
        mb.Entity<Device>().HasData(
            // Lights
            new Device { Id = 1,  Name = "ไฟเพดาน",        Type = DeviceType.Light,  RoomId = 1, IsOnline = true },
            new Device { Id = 2,  Name = "ไฟอ่านหนังสือ",  Type = DeviceType.Light,  RoomId = 1, IsOnline = true },
            new Device { Id = 3,  Name = "ไฟเพดาน",        Type = DeviceType.Light,  RoomId = 2, IsOnline = true },
            new Device { Id = 4,  Name = "ไฟหัวเตียง",     Type = DeviceType.Light,  RoomId = 2, IsOnline = true },
            // ACs
            new Device { Id = 5,  Name = "แอร์",           Type = DeviceType.AC,     RoomId = 1, IsOnline = true },
            new Device { Id = 6,  Name = "แอร์",           Type = DeviceType.AC,     RoomId = 2, IsOnline = true },
            new Device { Id = 7,  Name = "แอร์",           Type = DeviceType.AC,     RoomId = 5, IsOnline = true },
            new Device { Id = 8,  Name = "แอร์",           Type = DeviceType.AC,     RoomId = 6, IsOnline = true },
            // Doors
            new Device { Id = 9,  Name = "ประตูหน้า",      Type = DeviceType.Door,   RoomId = 1, IsOnline = true },
            new Device { Id = 10, Name = "ประตูหลัง",      Type = DeviceType.Door,   RoomId = 1, IsOnline = true },
            new Device { Id = 11, Name = "ประตูโรงรถ",     Type = DeviceType.Door,   RoomId = 7, IsOnline = true },
            new Device { Id = 12, Name = "ประตูห้องนอน",   Type = DeviceType.Door,   RoomId = 2, IsOnline = true },
            // Cameras
            new Device { Id = 13, Name = "กล้องหน้าบ้าน",  Type = DeviceType.Camera, RoomId = 1, IsOnline = true },
            new Device { Id = 14, Name = "กล้องห้องนั่งเล่น", Type = DeviceType.Camera, RoomId = 1, IsOnline = true },
            new Device { Id = 15, Name = "กล้องสวน",       Type = DeviceType.Camera, RoomId = 8, IsOnline = true },
            new Device { Id = 16, Name = "กล้องโรงรถ",     Type = DeviceType.Camera, RoomId = 7, IsOnline = true }
        );

        // Seed device states
        mb.Entity<DeviceState>().HasData(
            new DeviceState { Id = 1,  DeviceId = 1,  IsOn = true,  Brightness = 0.8 },
            new DeviceState { Id = 2,  DeviceId = 2,  IsOn = false, Brightness = 0.5 },
            new DeviceState { Id = 3,  DeviceId = 3,  IsOn = true,  Brightness = 0.6 },
            new DeviceState { Id = 4,  DeviceId = 4,  IsOn = true,  Brightness = 0.3 },
            new DeviceState { Id = 5,  DeviceId = 5,  IsOn = true,  SetTemperature = 24, CurrentTemperature = 26, AcMode = "cool", FanSpeed = 2 },
            new DeviceState { Id = 6,  DeviceId = 6,  IsOn = true,  SetTemperature = 25, CurrentTemperature = 27, AcMode = "cool", FanSpeed = 1 },
            new DeviceState { Id = 7,  DeviceId = 7,  IsOn = false, SetTemperature = 26, CurrentTemperature = 30, AcMode = "cool", FanSpeed = 2 },
            new DeviceState { Id = 8,  DeviceId = 8,  IsOn = false, SetTemperature = 26, CurrentTemperature = 28, AcMode = "cool", FanSpeed = 2 },
            new DeviceState { Id = 9,  DeviceId = 9,  IsOn = true,  IsLocked = true  },
            new DeviceState { Id = 10, DeviceId = 10, IsOn = true,  IsLocked = true  },
            new DeviceState { Id = 11, DeviceId = 11, IsOn = true,  IsLocked = false },
            new DeviceState { Id = 12, DeviceId = 12, IsOn = true,  IsLocked = true  },
            new DeviceState { Id = 13, DeviceId = 13, IsOn = true,  IsRecording = true,  HasMotion = true  },
            new DeviceState { Id = 14, DeviceId = 14, IsOn = true,  IsRecording = true,  HasMotion = false },
            new DeviceState { Id = 15, DeviceId = 15, IsOn = true,  IsRecording = true,  IsNightVision = true,  HasMotion = false },
            new DeviceState { Id = 16, DeviceId = 16, IsOn = true,  IsRecording = true,  HasMotion = false }
        );
    }
}
