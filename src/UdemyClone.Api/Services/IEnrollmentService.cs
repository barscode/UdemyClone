using UdemyClone.Api.Domain;

namespace UdemyClone.Api.Services;

public interface IEnrollmentService
{
    Task<(bool Success, string? Error, Enrollment? Enrollment)> EnrollAsync(int userId, int courseId);
}
