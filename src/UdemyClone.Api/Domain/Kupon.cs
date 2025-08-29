using System.ComponentModel.DataAnnotations;

namespace UdemyClone.Api.Domain;

public class Kupon
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; } // 0-100
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsActive { get; set; }
    public int? MaxUses { get; set; }
    public int? TimesUsed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Instructor who created the coupon
    public int? InstructorId { get; set; }
    public User? Instructor { get; set; }
    // Many-to-many: which courses this coupon applies to (if empty -> global for instructor)
    public ICollection<KuponKurs> KuponKurslar { get; set; } = new List<KuponKurs>();
}
