namespace IAutor.Api.Data.Entities;

public class PlanChapter : Base
{
    public long PlanId { get; set; }
    public long ChapterId { get; set; }

    [JsonIgnore] public Plan? Plan { get; set; }
    public Chapter? Chapter { get; set; }
    public ICollection<PlanChapterQuestion>? PlanChapterQuestions { get; set; }
}
