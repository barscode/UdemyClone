namespace UdemyClone.Api.DTOs;

public class AddLessonRequest
{
    public string Title { get; set; } = string.Empty;
    public string? ContentUrl { get; set; }
    public int DurationSeconds { get; set; }
}

