using UdemyClone.Api.Domain;
using UdemyClone.Api.Infrastructure.Repositories;

namespace UdemyClone.Api.Services;

public class MessageService : IMessageService
{
    private readonly IGenericRepository<Mesaj> _msgRepo;
    private readonly IGenericRepository<User> _userRepo;

    public MessageService(IGenericRepository<Mesaj> msgRepo, IGenericRepository<User> userRepo)
    {
        _msgRepo = msgRepo;
        _userRepo = userRepo;
    }

    public async Task<(bool Success, string? Error, Mesaj? Message)> SendMessageAsync(int senderId, int recipientId, string title, string content)
    {
        var sender = await _userRepo.GetByIdAsync(senderId);
        var recipient = await _userRepo.GetByIdAsync(recipientId);
        if (sender is null || recipient is null) return (false, "Kullanıcı bulunamadı.", null);

        var msg = new Mesaj { SenderId = senderId, RecipientId = recipientId, Baslik = title, Icerik = content, GonderimTarihi = DateTime.UtcNow };
        await _msgRepo.AddAsync(msg);
        await _msgRepo.SaveChangesAsync();
        return (true, null, msg);
    }
}
