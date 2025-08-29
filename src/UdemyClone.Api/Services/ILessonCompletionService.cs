using UdemyClone.Api.Domain;

namespace UdemyClone.Api.Services;

public interface ILessonCompletionService
{
    Task<(bool Success, string? Error, DersTamamlama? Completion)> MarkCompletedAsync(int userId, int lessonId);
}
