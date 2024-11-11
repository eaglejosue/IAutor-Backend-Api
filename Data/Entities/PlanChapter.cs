namespace IAutor.Api.Data.Entities;

public class PlanChapter
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public long PlanId { get; set; }
    public long ChapterId { get; set; }

    [JsonIgnore] public Plan? Plan { get; set; }
    public Chapter? Chapter { get; set; }
    public ICollection<PlanChapterQuestion>? PlanChapterQuestions { get; set; }
}
