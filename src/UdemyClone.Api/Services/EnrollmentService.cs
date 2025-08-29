using Microsoft.EntityFrameworkCore;
using UdemyClone.Api.Domain;
using UdemyClone.Api.Infrastructure.Repositories;

namespace UdemyClone.Api.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IGenericRepository<Enrollment> _enrRepo;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Course> _courseRepo;

    public EnrollmentService(IGenericRepository<Enrollment> enrRepo, IGenericRepository<User> userRepo, IGenericRepository<Course> courseRepo)
    {
        _enrRepo = enrRepo;
        _userRepo = userRepo;
        _courseRepo = courseRepo;
    }

    public async Task<(bool Success, string? Error, Enrollment? Enrollment)> EnrollAsync(int userId, int courseId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        var course = await _courseRepo.GetByIdAsync(courseId);
        if (user is null || course is null) return (false, "Kullanıcı veya kurs bulunamadı.", null);

        var exists = await _enrRepo.Query().AnyAsync(e => e.UserId == userId && e.CourseId == courseId);
        if (exists) return (false, "Zaten kayıtlı.", null);

        var enr = new Enrollment { UserId = userId, CourseId = courseId, EnrolledAt = DateTime.UtcNow };
        await _enrRepo.AddAsync(enr);
        await _enrRepo.SaveChangesAsync();
        return (true, null, enr);
    }
}
