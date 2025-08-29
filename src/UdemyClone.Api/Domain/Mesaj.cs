using System.ComponentModel.DataAnnotations;

namespace UdemyClone.Api.Domain;

public class Mesaj
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
    [Required]
    public string Baslik { get; set; } = string.Empty;
    [Required]
    public string Icerik { get; set; } = string.Empty;
    public DateTime GonderimTarihi { get; set; }

    // Navigation
    public User? Sender { get; set; }
    public User? Recipient { get; set; }
}
