using LumiHome.API.Data;
using LumiHome.API.Models;
using LumiHome.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumiHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext db, JwtService jwt) : ControllerBase
{
    public record RegisterRequest(string Username, string Email, string Password, string? DisplayName);
    public record LoginRequest(string Email, string Password);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        if (await db.Users.AnyAsync(u => u.Email == req.Email))
            return BadRequest(new { message = "Email นี้ถูกใช้แล้ว" });

        if (await db.Users.AnyAsync(u => u.Username == req.Username))
            return BadRequest(new { message = "Username นี้ถูกใช้แล้ว" });

        var user = new User
        {
            Username     = req.Username,
            Email        = req.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            DisplayName  = req.DisplayName ?? req.Username,
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Ok(new { token = jwt.GenerateToken(user), user = ToDto(user) });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { message = "Email หรือ Password ไม่ถูกต้อง" });

        user.LastLoginAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return Ok(new { token = jwt.GenerateToken(user), user = ToDto(user) });
    }

    private static object ToDto(User u) => new
    {
        u.Id, u.Username, u.Email, u.DisplayName, u.Role, u.CreatedAt
    };
}
