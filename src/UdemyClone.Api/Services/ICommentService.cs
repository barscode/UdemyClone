using UdemyClone.Api.Domain;

namespace UdemyClone.Api.Services;

public interface ICommentService
{
    Task<(bool Success, string? Error, Yorum? Comment)> AddCommentAsync(int authorId, int courseId, string content, int rating);
}
