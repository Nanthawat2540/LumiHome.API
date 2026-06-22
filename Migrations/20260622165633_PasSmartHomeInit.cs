using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PasTech.SmartHome.API.Migrations
{
    /// <inheritdoc />
    public partial class PasSmartHomeInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Automations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    TriggerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTriggeredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TriggerCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Automations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    FloorPlanX = table.Column<double>(type: "float", nullable: true),
                    FloorPlanY = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActivatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MacAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MqttTopic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    MqttTopic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    LastValue = table.Column<double>(type: "float", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastReadingAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ActionUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedDeviceId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutomationActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutomationId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetDeviceId = table.Column<int>(type: "int", nullable: true),
                    ActionConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelaySeconds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomationActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomationActions_Automations_AutomationId",
                        column: x => x.AutomationId,
                        principalTable: "Automations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutomationActions_Devices_TargetDeviceId",
                        column: x => x.TargetDeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DeviceLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceLogs_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DeviceStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    IsOn = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Brightness = table.Column<double>(type: "float", nullable: false),
                    ColorR = table.Column<int>(type: "int", nullable: false),
                    ColorG = table.Column<int>(type: "int", nullable: false),
                    ColorB = table.Column<int>(type: "int", nullable: false),
                    ColorTemp = table.Column<int>(type: "int", nullable: false),
                    SetTemperature = table.Column<double>(type: "float", nullable: false),
                    CurrentTemperature = table.Column<double>(type: "float", nullable: false),
                    AcMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FanSpeed = table.Column<int>(type: "int", nullable: false),
                    SwingMode = table.Column<bool>(type: "bit", nullable: false),
                    TimerMinutes = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    LastAccessBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRecording = table.Column<bool>(type: "bit", nullable: false),
                    IsNightVision = table.Column<bool>(type: "bit", nullable: false),
                    HasMotion = table.Column<bool>(type: "bit", nullable: false),
                    StreamUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SnapshotUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerWatts = table.Column<double>(type: "float", nullable: false),
                    Voltage = table.Column<double>(type: "float", nullable: false),
                    Current = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceStates_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnergyUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    KiloWattHours = table.Column<double>(type: "float", nullable: false),
                    CostBaht = table.Column<double>(type: "float", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnergyUsages_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAcknowledged = table.Column<bool>(type: "bit", nullable: false),
                    AcknowledgedByUserId = table.Column<int>(type: "int", nullable: true),
                    AcknowledgedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityEvents_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SensorReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorReadings_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CreatedAt", "Floor", "FloorPlanX", "FloorPlanY", "Icon", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, "🛋️", "ห้องนั่งเล่น" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, "🛏️", "ห้องนอนหลัก" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, "🍳", "ห้องครัว" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, "🚿", "ห้องน้ำ" },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, "💼", "ห้องทำงาน" },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, "🧸", "ห้องนอนลูก" },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, null, "🚗", "โรงรถ" },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, null, null, "🌿", "สวน" }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "Brand", "CreatedAt", "IpAddress", "IsOnline", "LastSeenAt", "MacAddress", "Model", "MqttTopic", "Name", "RoomId", "SerialNumber", "Type" },
                values: new object[,]
                {
                    { 1, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/1/state", "ไฟห้องนั่งเล่น", 1, null, "Light" },
                    { 2, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/2/state", "ไฟห้องนอน", 2, null, "Light" },
                    { 3, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/3/state", "ไฟห้องครัว", 3, null, "Light" },
                    { 4, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/4/state", "ไฟห้องทำงาน", 5, null, "Light" },
                    { 5, "Daikin", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/5/state", "แอร์ห้องนั่งเล่น", 1, null, "AC" },
                    { 6, "Mitsubishi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/6/state", "แอร์ห้องนอน", 2, null, "AC" },
                    { 7, "LG", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/7/state", "แอร์ห้องทำงาน", 5, null, "AC" },
                    { 8, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/8/state", "ประตูหน้าบ้าน", 1, null, "Door" },
                    { 9, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/9/state", "ประตูโรงรถ", 7, null, "Door" },
                    { 10, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/10/state", "ประตูหลัง", 8, null, "Door" },
                    { 11, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/11/state", "กล้องหน้าบ้าน", 1, null, "Camera" },
                    { 12, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/12/state", "กล้องสวนหลัง", 8, null, "Camera" },
                    { 13, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/13/state", "กล้องโรงรถ", 7, null, "Camera" },
                    { 14, "Generic", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, null, null, null, "pas/devices/14/state", "กล้องห้องนั่งเล่น", 1, null, "Camera" }
                });

            migrationBuilder.InsertData(
                table: "Sensors",
                columns: new[] { "Id", "CreatedAt", "IsOnline", "LastReadingAt", "LastValue", "MqttTopic", "Name", "RoomId", "Type", "Unit" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 26.5, "pas/sensors/1", "อุณหภูมิห้องนั่งเล่น", 1, "Temperature", "°C" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 62.0, "pas/sensors/2", "ความชื้นห้องนั่งเล่น", 1, "Humidity", "%" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0, "pas/sensors/3", "เซนเซอร์การเคลื่อนไหว", 1, "Motion", "" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0, "pas/sensors/4", "เซนเซอร์ควัน", 3, "Smoke", "ppm" },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0, "pas/sensors/5", "เซนเซอร์น้ำรั่ว", 4, "WaterLeak", "" }
                });

            migrationBuilder.InsertData(
                table: "DeviceStates",
                columns: new[] { "Id", "AcMode", "AccessMethod", "Brightness", "ColorB", "ColorG", "ColorR", "ColorTemp", "Current", "CurrentTemperature", "DeviceId", "FanSpeed", "HasMotion", "IsLocked", "IsNightVision", "IsOn", "IsRecording", "LastAccessBy", "PowerWatts", "SetTemperature", "SnapshotUrl", "StreamUrl", "SwingMode", "TimerMinutes", "UpdatedAt", "Voltage" },
                values: new object[,]
                {
                    { 1, "cool", null, 0.80000000000000004, 255, 255, 255, 4000, 0.0, 28.0, 1, 2, false, true, false, true, false, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 2, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 28.0, 2, 2, false, true, false, false, false, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 3, "cool", null, 0.59999999999999998, 255, 255, 255, 4000, 0.0, 28.0, 3, 2, false, true, false, true, false, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 4, "cool", null, 0.90000000000000002, 255, 255, 255, 4000, 0.0, 28.0, 4, 2, false, true, false, true, false, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 5, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 26.0, 5, 2, false, true, false, true, false, null, 0.0, 24.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 6, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 28.0, 6, 1, false, true, false, false, false, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 7, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 27.0, 7, 3, false, true, false, true, false, null, 0.0, 23.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 8, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 28.0, 8, 2, false, true, false, false, false, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 9, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 28.0, 9, 2, false, false, false, false, false, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 10, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 28.0, 10, 2, false, true, false, false, false, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 11, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 28.0, 11, 2, true, true, false, false, true, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 12, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 28.0, 12, 2, false, true, true, false, true, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 13, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 28.0, 13, 2, false, true, false, false, true, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 },
                    { 14, "cool", null, 1.0, 255, 255, 255, 4000, 0.0, 28.0, 14, 2, false, true, true, false, true, null, 0.0, 25.0, null, null, false, 0, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutomationActions_AutomationId",
                table: "AutomationActions",
                column: "AutomationId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationActions_TargetDeviceId",
                table: "AutomationActions",
                column: "TargetDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogs_DeviceId",
                table: "DeviceLogs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogs_UserId",
                table: "DeviceLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_RoomId",
                table: "Devices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceStates_DeviceId",
                table: "DeviceStates",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnergyUsages_DeviceId",
                table: "EnergyUsages",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityEvents_DeviceId",
                table: "SecurityEvents",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_SensorId",
                table: "SensorReadings",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_RoomId",
                table: "Sensors",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutomationActions");

            migrationBuilder.DropTable(
                name: "DeviceLogs");

            migrationBuilder.DropTable(
                name: "DeviceStates");

            migrationBuilder.DropTable(
                name: "EnergyUsages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Scenes");

            migrationBuilder.DropTable(
                name: "SecurityEvents");

            migrationBuilder.DropTable(
                name: "SensorReadings");

            migrationBuilder.DropTable(
                name: "Automations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
