using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdemyClone.Api.Domain;

[Table("Kategoriler")]
public class Kategori
{
    public int Id { get; set; }
    [Required]
    public string Ad { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
    public string? Ikon { get; set; }

    // Navigation properties
    public ICollection<Kurs> Kurslar { get; set; } = new List<Kurs>();
    public ICollection<Kategori>? AltKategoriler { get; set; }
    public Kategori? UstKategori { get; set; }
    public int? UstKategoriId { get; set; }
}
