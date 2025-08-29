using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyClone.Api.Data;

namespace UdemyClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public UsersController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("{userId:int}/courses")]
    public async Task<IActionResult> GetCourses(int userId)
    {
        var data = await _db.Enrollments
            .Where(e => e.UserId == userId)
            .Select(e => new { e.Course!.Id, e.Course.Baslik, e.EnrolledAt })
            .ToListAsync();
        return Ok(data);
    }
}
