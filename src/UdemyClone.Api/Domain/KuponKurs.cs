namespace UdemyClone.Api.Domain;

public class KuponKurs
{
    public int KuponId { get; set; }
    public Kupon? Kupon { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }
}
