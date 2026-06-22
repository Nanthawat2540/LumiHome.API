using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LumiHome.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
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
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
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
                name: "DeviceLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
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
                    SetTemperature = table.Column<double>(type: "float", nullable: false),
                    CurrentTemperature = table.Column<double>(type: "float", nullable: false),
                    AcMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FanSpeed = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    IsRecording = table.Column<bool>(type: "bit", nullable: false),
                    IsNightVision = table.Column<bool>(type: "bit", nullable: false),
                    HasMotion = table.Column<bool>(type: "bit", nullable: false)
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
                name: "SecurityEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAcknowledged = table.Column<bool>(type: "bit", nullable: false),
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

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CreatedAt", "Floor", "Icon", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 22, 15, 21, 27, 122, DateTimeKind.Utc).AddTicks(7465), 1, "🛋️", "ห้องนั่งเล่น" },
                    { 2, new DateTime(2026, 6, 22, 15, 21, 27, 122, DateTimeKind.Utc).AddTicks(9189), 2, "🛏️", "ห้องนอนหลัก" },
                    { 3, new DateTime(2026, 6, 22, 15, 21, 27, 122, DateTimeKind.Utc).AddTicks(9193), 1, "🍳", "ห้องครัว" },
                    { 4, new DateTime(2026, 6, 22, 15, 21, 27, 122, DateTimeKind.Utc).AddTicks(9194), 1, "🚿", "ห้องน้ำ" },
                    { 5, new DateTime(2026, 6, 22, 15, 21, 27, 122, DateTimeKind.Utc).AddTicks(9196), 2, "💻", "ห้องทำงาน" },
                    { 6, new DateTime(2026, 6, 22, 15, 21, 27, 122, DateTimeKind.Utc).AddTicks(9232), 2, "🧸", "ห้องนอนลูก" },
                    { 7, new DateTime(2026, 6, 22, 15, 21, 27, 122, DateTimeKind.Utc).AddTicks(9233), 0, "🚗", "โรงรถ" },
                    { 8, new DateTime(2026, 6, 22, 15, 21, 27, 122, DateTimeKind.Utc).AddTicks(9235), 0, "🌿", "สวน" }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "CreatedAt", "IpAddress", "IsOnline", "Name", "RoomId", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 22, 15, 21, 27, 123, DateTimeKind.Utc).AddTicks(8753), null, true, "ไฟเพดาน", 1, "Light" },
                    { 2, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(815), null, true, "ไฟอ่านหนังสือ", 1, "Light" },
                    { 3, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(819), null, true, "ไฟเพดาน", 2, "Light" },
                    { 4, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(821), null, true, "ไฟหัวเตียง", 2, "Light" },
                    { 5, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(823), null, true, "แอร์", 1, "AC" },
                    { 6, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(824), null, true, "แอร์", 2, "AC" },
                    { 7, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(827), null, true, "แอร์", 5, "AC" },
                    { 8, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(828), null, true, "แอร์", 6, "AC" },
                    { 9, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(829), null, true, "ประตูหน้า", 1, "Door" },
                    { 10, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(831), null, true, "ประตูหลัง", 1, "Door" },
                    { 11, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(832), null, true, "ประตูโรงรถ", 7, "Door" },
                    { 12, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(834), null, true, "ประตูห้องนอน", 2, "Door" },
                    { 13, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(835), null, true, "กล้องหน้าบ้าน", 1, "Camera" },
                    { 14, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(837), null, true, "กล้องห้องนั่งเล่น", 1, "Camera" },
                    { 15, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(838), null, true, "กล้องสวน", 8, "Camera" },
                    { 16, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(840), null, true, "กล้องโรงรถ", 7, "Camera" }
                });

            migrationBuilder.InsertData(
                table: "DeviceStates",
                columns: new[] { "Id", "AcMode", "Brightness", "CurrentTemperature", "DeviceId", "FanSpeed", "HasMotion", "IsLocked", "IsNightVision", "IsOn", "IsRecording", "SetTemperature", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "cool", 0.80000000000000004, 28.0, 1, 2, false, true, false, true, false, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(2241) },
                    { 2, "cool", 0.5, 28.0, 2, 2, false, true, false, false, false, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(3615) },
                    { 3, "cool", 0.59999999999999998, 28.0, 3, 2, false, true, false, true, false, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(3616) },
                    { 4, "cool", 0.29999999999999999, 28.0, 4, 2, false, true, false, true, false, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(3618) },
                    { 5, "cool", 1.0, 26.0, 5, 2, false, true, false, true, false, 24.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(3621) },
                    { 6, "cool", 1.0, 27.0, 6, 1, false, true, false, true, false, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(4895) },
                    { 7, "cool", 1.0, 30.0, 7, 2, false, true, false, false, false, 26.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(4898) },
                    { 8, "cool", 1.0, 28.0, 8, 2, false, true, false, false, false, 26.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(4899) },
                    { 9, "cool", 1.0, 28.0, 9, 2, false, true, false, true, false, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(4901) },
                    { 10, "cool", 1.0, 28.0, 10, 2, false, true, false, true, false, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(5187) },
                    { 11, "cool", 1.0, 28.0, 11, 2, false, false, false, true, false, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(5188) },
                    { 12, "cool", 1.0, 28.0, 12, 2, false, true, false, true, false, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(5189) },
                    { 13, "cool", 1.0, 28.0, 13, 2, true, true, false, true, true, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(5190) },
                    { 14, "cool", 1.0, 28.0, 14, 2, false, true, false, true, true, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(5971) },
                    { 15, "cool", 1.0, 28.0, 15, 2, false, true, true, true, true, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(5973) },
                    { 16, "cool", 1.0, 28.0, 16, 2, false, true, false, true, true, 25.0, new DateTime(2026, 6, 22, 15, 21, 27, 124, DateTimeKind.Utc).AddTicks(6467) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogs_DeviceId",
                table: "DeviceLogs",
                column: "DeviceId");

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
                name: "IX_SecurityEvents_DeviceId",
                table: "SecurityEvents",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceLogs");

            migrationBuilder.DropTable(
                name: "DeviceStates");

            migrationBuilder.DropTable(
                name: "SecurityEvents");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
