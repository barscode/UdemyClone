using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdemyClone.Api.Domain;

public class Kurs
{
    public int Id { get; set; }
    [Required]
    public string Baslik { get; set; } = string.Empty;
    [Required]
    public string Aciklama { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Fiyat { get; set; }
    public bool YayindaMi { get; set; }
    [Required]
    public string KapakResmi { get; set; } = string.Empty;
    public string? TanitimVideosu { get; set; }
    public string Seviye { get; set; } = "Başlangıç"; // Başlangıç, Orta, İleri
    public DateTime OlusturulmaTarihi { get; set; }
    public DateTime? GuncellenmeTarihi { get; set; }
    public int TahminiSure { get; set; } // Dakika cinsinden

    // Navigation properties
    public int EgitmenId { get; set; }
    public Kullanici Egitmen { get; set; } = null!;
    public int KategoriId { get; set; }
    public Kategori Kategori { get; set; } = null!;
    public ICollection<Ders> Dersler { get; set; } = new List<Ders>();
    public ICollection<KursKayit> Kayitlar { get; set; } = new List<KursKayit>();
    public ICollection<Yorum> Yorumlar { get; set; } = new List<Yorum>();
    public ICollection<Kupon> Kuponlar { get; set; } = new List<Kupon>();
}
