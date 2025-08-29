using UdemyClone.Api.Domain;
using UdemyClone.Api.Infrastructure.Repositories;

namespace UdemyClone.Api.Services;

public class CommentService : ICommentService
{
    private readonly IGenericRepository<Yorum> _yorumRepo;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Course> _courseRepo;

    public CommentService(IGenericRepository<Yorum> yorumRepo, IGenericRepository<User> userRepo, IGenericRepository<Course> courseRepo)
    {
        _yorumRepo = yorumRepo;
        _userRepo = userRepo;
        _courseRepo = courseRepo;
    }

    public async Task<(bool Success, string? Error, Yorum? Comment)> AddCommentAsync(int authorId, int courseId, string content, int rating)
    {
        var author = await _userRepo.GetByIdAsync(authorId);
        var course = await _courseRepo.GetByIdAsync(courseId);
        if (author is null || course is null) return (false, "Kullanıcı veya kurs bulunamadı.", null);
        if (rating < 1 || rating > 5) return (false, "Puan 1-5 aralığında olmalıdır.", null);

        var yorum = new Yorum { AuthorId = authorId, CourseId = courseId, Icerik = content, Puan = rating, Tarih = DateTime.UtcNow };
        await _yorumRepo.AddAsync(yorum);
        await _yorumRepo.SaveChangesAsync();
        return (true, null, yorum);
    }
}
