using System.ComponentModel.DataAnnotations;

namespace UdemyClone.Api.Domain;

public class Yorum
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int AuthorId { get; set; }
    [Required]
    public string Icerik { get; set; } = string.Empty;
    public int Puan { get; set; } // 1-5
    public DateTime Tarih { get; set; }

    // Navigation
    public Course? Course { get; set; }
    public User? Author { get; set; }
}
