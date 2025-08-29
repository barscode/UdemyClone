namespace UdemyClone.Api.DTOs;

public class CreateCouponRequest
{
    public int InstructorId { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int? MaxUses { get; set; }
    public int[]? CourseIds { get; set; } // null or empty = global for instructor
}
