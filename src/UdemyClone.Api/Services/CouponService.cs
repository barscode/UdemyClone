using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UdemyClone.Api.Domain;
using UdemyClone.Api.DTOs;
using UdemyClone.Api.Infrastructure.Repositories;

namespace UdemyClone.Api.Services;

public class CouponService : ICouponService
{
    private readonly IGenericRepository<Kupon> _kuponRepo;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Course> _courseRepo;
    private readonly IGenericRepository<KuponKurs> _kuponKursRepo;

    public CouponService(IGenericRepository<Kupon> kuponRepo,
        IGenericRepository<User> userRepo,
        IGenericRepository<Course> courseRepo,
        IGenericRepository<KuponKurs> kuponKursRepo)
    {
        _kuponRepo = kuponRepo;
        _userRepo = userRepo;
        _courseRepo = courseRepo;
        _kuponKursRepo = kuponKursRepo;
    }

    public async Task<(bool Success, string? Error, Kupon? Coupon)> CreateCouponAsync(CreateCouponRequest req)
    {
        var instructor = await _userRepo.GetByIdAsync(req.InstructorId);
        if (instructor is null || instructor.Rol != UserRoles.Instructor)
            return (false, "Geçersiz eğitmen.", null);

        var since = DateTime.UtcNow.AddDays(-30);
        var count = await _kuponRepo.Query().CountAsync(k => k.InstructorId == req.InstructorId && k.CreatedAt >= since);
        if (count >= 3) return (false, "Son 30 günde en fazla 3 kupon oluşturabilirsiniz.", null);

        var kupon = new Kupon
        {
            Code = req.Code,
            DiscountPercent = req.DiscountPercent,
            Start = req.Start,
            End = req.End,
            MaxUses = req.MaxUses,
            IsActive = true,
            InstructorId = req.InstructorId
        };

        await _kuponRepo.AddAsync(kupon);
        await _kuponRepo.SaveChangesAsync();

        if (req.CourseIds != null && req.CourseIds.Length > 0)
        {
            var courses = await _courseRepo.Query().Where(c => req.CourseIds.Contains(c.Id)).ToListAsync();
            if (courses.Any(c => c.EgitmenId != req.InstructorId))
                return (false, "Sadece kendi kurslarınıza kupon tanımlayabilirsiniz.", null);

            foreach (var cid in req.CourseIds)
            {
                await _kuponKursRepo.AddAsync(new KuponKurs { KuponId = kupon.Id, CourseId = cid });
            }
            await _kuponKursRepo.SaveChangesAsync();
        }

        return (true, null, kupon);
    }
}
