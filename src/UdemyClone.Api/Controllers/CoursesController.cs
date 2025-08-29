using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UdemyClone.Api.Data;
using UdemyClone.Api.Domain;
using UdemyClone.Api.DTOs;
using UdemyClone.Api.Services;

namespace UdemyClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ICommentService _commentService;

    public CoursesController(ApplicationDbContext db, ICommentService commentService)
    {
        _db = db;
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest req)
    {
        var instructor = await _db.Users.FindAsync(req.InstructorId);
        if (instructor is null || instructor.Rol != UserRoles.Instructor)
            return BadRequest("Geçersiz eğitmen.");
        var kategori = await _db.Categories.FindAsync(req.CategoryId);
        if (kategori is null) return BadRequest("Geçersiz kategori.");

        var course = new Course
        {
            Baslik = req.Title,
            Aciklama = req.Description,
            Fiyat = req.Price,
            EgitmenId = req.InstructorId,
            KategoriId = req.CategoryId,
            YayindaMi = req.Published
        };
        _db.Courses.Add(course);
        await _db.SaveChangesAsync();
        return Created($"/api/courses/{course.Id}", course);
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] int? categoryId, [FromQuery] string? q)
    {
        var query = _db.Courses
            .Include(c => c.Kategori)
            .Include(c => c.Egitmen)
            .AsQueryable();

        if (categoryId.HasValue) query = query.Where(c => c.KategoriId == categoryId.Value);
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(c => c.Baslik.Contains(q));

        var data = await query
            .Select(c => new
            {
                c.Id,
                c.Baslik,
                c.Fiyat,
                Category = c.Kategori!.Ad,
                Instructor = c.Egitmen!.Email,
                Lessons = c.Dersler.Count
            }).ToListAsync();

        return Ok(data);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var course = await _db.Courses
            .Include(c => c.Kategori)
            .Include(c => c.Egitmen)
            .Include(c => c.Dersler)
            .FirstOrDefaultAsync(c => c.Id == id);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpPost("{id:int}/lessons")]
    public async Task<IActionResult> AddLesson(int id, [FromBody] AddLessonRequest req)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course is null) return NotFound();

        var lesson = new Lesson
        {
            KursId = id,
            Baslik = req.Title,
            IcerikUrl = req.ContentUrl,
            SureSaniye = req.DurationSeconds
        };
        _db.Lessons.Add(lesson);
        await _db.SaveChangesAsync();
        return Created($"/api/courses/{id}/lessons/{lesson.Id}", lesson);
    }

    [Authorize]
    [HttpPost("{courseId:int}/comments")]
    public async Task<IActionResult> AddComment(int courseId, string content, int rating)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(sub, out var authorId)) return Unauthorized();
        var (ok, err, comment) = await _commentService.AddCommentAsync(authorId, courseId, content, rating);
        return ok
            ? Created($"/api/courses/{courseId}/comments/{comment!.Id}", comment)
            : BadRequest(err);
    }
}
