using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyClone.Api.Data;
using UdemyClone.Api.Domain;
using UdemyClone.Api.DTOs;

namespace UdemyClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public AuthController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (await _db.Users.AnyAsync(u => u.Email == req.Email))
            return BadRequest("Email zaten kayıtlı.");

        var user = new User
        {
            Email = req.Email,
            SifreHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Rol = string.IsNullOrWhiteSpace(req.Role) ? UserRoles.Student : req.Role!
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Created($"/api/users/{user.Id}", new { user.Id, user.Email, user.Rol });
    }
}
