namespace IAutor.Api.Data.Entities;

public class PlanChapterQuestion
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public long PlanChapterId { get; set; }
    public long QuestionId { get; set; }

    [JsonIgnore] public PlanChapter? PlanChapter { get; set; }
    public Question? Question { get; set; }
}
