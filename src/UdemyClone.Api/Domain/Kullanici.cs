using System.ComponentModel.DataAnnotations;

namespace UdemyClone.Api.Domain;

public class Kullanici
{
    public int Id { get; set; }
    [Required]
    public string Ad { get; set; } = string.Empty;
    [Required]
    public string Soyad { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string SifreHash { get; set; } = string.Empty;
    [Required]
    public string Rol { get; set; } = string.Empty; // Admin, Egitmen, Ogrenci
    public string? Fotograf { get; set; }
    public string? Hakkinda { get; set; }
    public DateTime KayitTarihi { get; set; }
    public DateTime SonGirisTarihi { get; set; }

    // Navigation properties
    public ICollection<Kurs> VerdigiKurslar { get; set; } = new List<Kurs>();
    public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();
    public ICollection<Mesaj> GonderilenMesajlar { get; set; } = new List<Mesaj>();
    public ICollection<Mesaj> AlinanMesajlar { get; set; } = new List<Mesaj>();
    public ICollection<Yorum> Yorumlar { get; set; } = new List<Yorum>();
}
