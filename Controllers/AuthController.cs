using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasTech.SmartHome.API.Data;
using PasTech.SmartHome.API.Models;
using PasTech.SmartHome.API.Services;

namespace PasTech.SmartHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext db, JwtService jwt) : ControllerBase
{
    public record RegisterRequest(string Username, string Email, string Password, string? DisplayName, string? PhoneNumber);
    public record LoginRequest(string Email, string Password);
    public record RefreshRequest(string RefreshToken);

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
            PhoneNumber  = req.PhoneNumber,
            Role         = db.Users.Any() ? UserRole.User : UserRole.Admin,
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Ok(await BuildAuthResponse(user));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { message = "Email หรือรหัสผ่านไม่ถูกต้อง" });
        if (!user.IsActive)
            return Unauthorized(new { message = "บัญชีนี้ถูกระงับ" });

        user.LastLoginAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(await BuildAuthResponse(user));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest req)
    {
        var token = await db.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == req.RefreshToken);

        if (token == null || !token.IsActive)
            return Unauthorized(new { message = "Refresh token ไม่ถูกต้องหรือหมดอายุ" });

        token.RevokedAt = DateTime.UtcNow;
        return Ok(await BuildAuthResponse(token.User));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(RefreshRequest req)
    {
        var token = await db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == req.RefreshToken);
        if (token != null) token.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Ok(new { message = "ออกจากระบบแล้ว" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var user = await db.Users.FindAsync(userId);
        return user == null ? NotFound() : Ok(ToDto(user));
    }

    private async Task<object> BuildAuthResponse(User user)
    {
        var refreshToken = jwt.GenerateRefreshToken(HttpContext.Connection.RemoteIpAddress?.ToString());
        refreshToken.UserId = user.Id;
        db.RefreshTokens.Add(refreshToken);

        // Revoke old tokens
        var oldTokens = await db.RefreshTokens
            .Where(r => r.UserId == user.Id && r.RevokedAt == null && r.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();
        foreach (var t in oldTokens) t.RevokedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return new
        {
            accessToken  = jwt.GenerateAccessToken(user),
            refreshToken = refreshToken.Token,
            expiresIn    = 3600,
            user         = ToDto(user),
        };
    }

    private static object ToDto(User u) => new
    {
        u.Id, u.Username, u.Email, u.DisplayName, u.PhoneNumber,
        u.AvatarUrl, Role = u.Role.ToString(), u.IsActive, u.CreatedAt, u.LastLoginAt,
    };
}
