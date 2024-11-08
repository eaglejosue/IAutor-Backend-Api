namespace IAutor.Api.Data.Entities;

public class PlanChapterQuestion : Base
{
    public long PlanChapterId { get; set; }
    public long QuestionId { get; set; }

    [JsonIgnore] public PlanChapter? PlanChapter { get; set; }
    public Question? Question { get; set; }
}
