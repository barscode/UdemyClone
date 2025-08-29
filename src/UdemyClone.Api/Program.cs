using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UdemyClone.Api.Data;
using UdemyClone.Api.Domain;
using UdemyClone.Api.DTOs;
using UdemyClone.Api.Application.UseCases;
using UdemyClone.Api.Infrastructure.Repositories;
using UdemyClone.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
 builder.Services.ConfigureHttpJsonOptions(options =>  {   options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;  });
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();
// Repositories and services
builder.Services.AddScoped(typeof(UdemyClone.Api.Infrastructure.Repositories.IGenericRepository<>), typeof(UdemyClone.Api.Infrastructure.Repositories.EfGenericRepository<>));
builder.Services.AddScoped<UdemyClone.Api.Application.UseCases.CreateCouponUseCase>();
// Application services
builder.Services.AddScoped<UdemyClone.Api.Services.IMessageService, UdemyClone.Api.Services.MessageService>();
builder.Services.AddScoped<UdemyClone.Api.Services.ICommentService, UdemyClone.Api.Services.CommentService>();
builder.Services.AddScoped<UdemyClone.Api.Services.IEnrollmentService, UdemyClone.Api.Services.EnrollmentService>();
builder.Services.AddScoped<UdemyClone.Api.Services.ILessonCompletionService, UdemyClone.Api.Services.LessonCompletionService>();

var app = builder.Build();

// DB init (EnsureCreated for quick start)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureDeleted(); // For development: delete existing database to apply new schema
    db.Database.EnsureCreated();
    if (!db.Set<UdemyClone.Api.Domain.Kategori>().Any())
    {
        db.Set<UdemyClone.Api.Domain.Kategori>().AddRange(
            new UdemyClone.Api.Domain.Kategori { Ad = "Development" },
            new UdemyClone.Api.Domain.Kategori { Ad = "Business" },
            new UdemyClone.Api.Domain.Kategori { Ad = "Design" }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Coupons endpoints
var coupons = app.MapGroup("/coupons");

coupons.MapPost("/", async (CreateCouponUseCase useCase, HttpContext http, CreateCouponRequest req) =>
{
    if (!http.User.Identity?.IsAuthenticated ?? true) return Results.Unauthorized();
    var sub = http.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(sub, out var instructorId)) return Results.Unauthorized();

    var (success, error, coupon) = await useCase.ExecuteAsync(instructorId, req);
    return success ? Results.Created($"/coupons/{coupon!.Id}", coupon) : Results.BadRequest(error);
}).RequireAuthorization();

// Auth endpoints
var auth = app.MapGroup("/auth");

// Auth endpoints - Register
auth.MapPost("/register", async (ApplicationDbContext db, RegisterRequest req) =>
{
    if (await db.Users.AnyAsync(u => u.Email == req.Email))
        return Results.BadRequest("Email zaten kayıtlı.");

    var user = new User
    {
        Email = req.Email,
        SifreHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
        Rol = string.IsNullOrWhiteSpace(req.Role) ? UserRoles.Student : req.Role!
    };
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", new { user.Id, user.Email, user.Rol });
});

// Auth endpoints - Login remains unchanged

// Messaging
app.MapPost("/messages", async (IMessageService svc, HttpContext http, int recipientId, string title, string content) =>
{
    if (!http.User.Identity?.IsAuthenticated ?? true) return Results.Unauthorized();
    var sub = http.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(sub, out var senderId)) return Results.Unauthorized();
    var (ok, err, msg) = await svc.SendMessageAsync(senderId, recipientId, title, content);
    return ok ? Results.Created($"/messages/{msg!.Id}", msg) : Results.BadRequest(err);
}).RequireAuthorization();

// Comments
app.MapPost("/courses/{courseId:int}/comments", async (ICommentService svc, HttpContext http, int courseId, string content, int rating) =>
{
    if (!http.User.Identity?.IsAuthenticated ?? true) return Results.Unauthorized();
    var sub = http.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(sub, out var authorId)) return Results.Unauthorized();
    var (ok, err, comment) = await svc.AddCommentAsync(authorId, courseId, content, rating);
    return ok ? Results.Created($"/courses/{courseId}/comments/{comment!.Id}", comment) : Results.BadRequest(err);
}).RequireAuthorization();

// Enrollment (wraps existing logic)
app.MapPost("/enrollments", async (IEnrollmentService svc, EnrollRequest req) =>
{
    var (ok, err, enr) = await svc.EnrollAsync(req.UserId, req.CourseId);
    return ok ? Results.Created($"/enrollments/{enr!.Id}", enr) : Results.BadRequest(err);
});

// Lesson completion
app.MapPost("/lessons/{lessonId:int}/complete", async (ILessonCompletionService svc, HttpContext http, int lessonId) =>
{
    if (!http.User.Identity?.IsAuthenticated ?? true) return Results.Unauthorized();
    var sub = http.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(sub, out var userId)) return Results.Unauthorized();
    var (ok, err, comp) = await svc.MarkCompletedAsync(userId, lessonId);
    return ok ? Results.Created($"/lessons/{lessonId}/complete/{comp!.Id}", comp) : Results.BadRequest(err);
}).RequireAuthorization();

// Courses endpoints
var courses = app.MapGroup("/courses");

// Courses endpoints - Create course endpoint
courses.MapPost("/", async (ApplicationDbContext db, CreateCourseRequest req) =>
{
    var instructor = await db.Users.FindAsync(req.InstructorId);
    if (instructor is null || instructor.Rol != UserRoles.Instructor)
        return Results.BadRequest("Geçersiz eğitmen.");
    var kategori = await db.Categories.FindAsync(req.CategoryId);
    if (kategori is null) return Results.BadRequest("Geçersiz kategori.");

    var course = new Course
    {
        Baslik = req.Title,
        Aciklama = req.Description,
        Fiyat = req.Price,
        EgitmenId = req.InstructorId,
        KategoriId = req.CategoryId,
        YayindaMi = req.Published
    };
    db.Courses.Add(course);
    await db.SaveChangesAsync();
    return Results.Created($"/courses/{course.Id}", course);
});

// Courses endpoints - Get courses mapping
courses.MapGet("/", async (ApplicationDbContext db, int? categoryId, string? q) =>
{
    var query = db.Courses
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

    return Results.Ok(data);
});

// Courses endpoints - Get course detail
courses.MapGet("/{id:int}", async (ApplicationDbContext db, int id) =>
{
    var course = await db.Courses
        .Include(c => c.Kategori)
        .Include(c => c.Egitmen)
        .Include(c => c.Dersler)
        .FirstOrDefaultAsync(c => c.Id == id);
    return course is null ? Results.NotFound() : Results.Ok(course);
});

// Lessons endpoint - Create lesson for a course
courses.MapPost("/{id:int}/lessons", async (ApplicationDbContext db, int id, AddLessonRequest req) =>
{
    var course = await db.Courses.FindAsync(id);
    if (course is null) return Results.NotFound();

    var lesson = new Lesson
    {
        KursId = id,
        Baslik = req.Title,
        IcerikUrl = req.ContentUrl,
        SureSaniye = req.DurationSeconds
    };
    db.Lessons.Add(lesson);
    await db.SaveChangesAsync();
    return Results.Created($"/courses/{id}/lessons/{lesson.Id}", lesson);
});

// Enrollment endpoints
var enrollments = app.MapGroup("/enrollments");
enrollments.MapPost("/", async (ApplicationDbContext db, EnrollRequest req) =>
{
    var user = await db.Users.FindAsync(req.UserId);
    var course = await db.Courses.FindAsync(req.CourseId);
    if (user is null || course is null) return Results.BadRequest("Geçersiz kullanıcı veya kurs.");
    if (await db.Enrollments.AnyAsync(e => e.UserId == req.UserId && e.CourseId == req.CourseId))
        return Results.Conflict("Zaten kayıtlısınız.");

    var enr = new Enrollment { UserId = req.UserId, CourseId = req.CourseId, EnrolledAt = DateTime.UtcNow };
    db.Enrollments.Add(enr);
    await db.SaveChangesAsync();
    return Results.Created($"/enrollments/{enr.Id}", enr);
});

app.MapGet("/users/{userId:int}/courses", async (ApplicationDbContext db, int userId) =>
{
    var data = await db.Enrollments
        .Where(e => e.UserId == userId)
        .Select(e => new { e.Course!.Id, e.Course.Baslik, e.EnrolledAt })
        .ToListAsync();
    return Results.Ok(data);
});

app.MapGet("/instructors/{instructorId:int}/courses", async (ApplicationDbContext db, int instructorId) =>
{
    var data = await db.Courses
        .Where(c => c.EgitmenId == instructorId)
        .Select(c => new { c.Id, c.Baslik, c.YayindaMi })
        .ToListAsync();
    return Results.Ok(data);
});

app.Run();

