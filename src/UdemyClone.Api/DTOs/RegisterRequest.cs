using Microsoft.EntityFrameworkCore.Diagnostics;

namespace UdemyClone.Api.DTOs;

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; }

    public string LastName { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? Role { get; set; } // Student | Instructor | Admin
}

