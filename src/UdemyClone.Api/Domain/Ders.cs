using System.ComponentModel.DataAnnotations;

namespace UdemyClone.Api.Domain;

public class Ders
{
    public int Id { get; set; }
    public int KursId { get; set; }
    [Required]
    public string Baslik { get; set; } = string.Empty;
    public string? IcerikUrl { get; set; }
    public int SureSaniye { get; set; }
    public string? Aciklama { get; set; }
    public bool UcretsizMi { get; set; }
    public int Sira { get; set; }

    // Navigation properties
    public Kurs Kurs { get; set; } = null!;
    public ICollection<DersTamamlama> Tamamlamalar { get; set; } = new List<DersTamamlama>();
}
