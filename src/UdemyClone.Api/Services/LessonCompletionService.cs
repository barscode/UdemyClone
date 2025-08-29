using Microsoft.EntityFrameworkCore;
using UdemyClone.Api.Domain;
using UdemyClone.Api.Infrastructure.Repositories;

namespace UdemyClone.Api.Services;

public class LessonCompletionService : ILessonCompletionService
{
    private readonly IGenericRepository<DersTamamlama> _compRepo;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Lesson> _lessonRepo;

    public LessonCompletionService(IGenericRepository<DersTamamlama> compRepo, IGenericRepository<User> userRepo, IGenericRepository<Lesson> lessonRepo)
    {
        _compRepo = compRepo;
        _userRepo = userRepo;
        _lessonRepo = lessonRepo;
    }

    public async Task<(bool Success, string? Error, DersTamamlama? Completion)> MarkCompletedAsync(int userId, int lessonId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        var lesson = await _lessonRepo.GetByIdAsync(lessonId);
        if (user is null || lesson is null) return (false, "Kullanıcı veya ders bulunamadı.", null);

        var exists = await _compRepo.Query().AnyAsync(d => d.UserId == userId && d.LessonId == lessonId);
        if (exists) return (false, "Zaten tamamlandı.", null);

        var comp = new DersTamamlama { UserId = userId, LessonId = lessonId, CompletedAt = DateTime.UtcNow };
        await _compRepo.AddAsync(comp);
        await _compRepo.SaveChangesAsync();
        return (true, null, comp);
    }
}
