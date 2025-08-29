using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyClone.Api.Data;

namespace UdemyClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public InstructorsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("{instructorId:int}/courses")]
    public async Task<IActionResult> GetCourses(int instructorId)
    {
        var data = await _db.Courses
            .Where(c => c.EgitmenId == instructorId)
            .Select(c => new { c.Id, c.Baslik, c.YayindaMi })
            .ToListAsync();
        return Ok(data);
    }
}
