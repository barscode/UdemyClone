namespace UdemyClone.Api.DTOs;

public class CreateCourseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int InstructorId { get; set; }
    public int CategoryId { get; set; }
    public bool Published { get; set; } = true;
}

