using UdemyClone.Api.Domain;

namespace UdemyClone.Api.Services;

public interface IMessageService
{
    Task<(bool Success, string? Error, Mesaj? Message)> SendMessageAsync(int senderId, int recipientId, string title, string content);
}
