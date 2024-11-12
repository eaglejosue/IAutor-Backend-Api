namespace IAutor.Api.Data.Entities;

public class Chapter : Base
{
    public string Title { get; set; }
    public int? ChapterNumber { get; set; }

    [JsonIgnore] public ICollection<PlanChapter>? PlanChapters { get; }

    //Used on front
    [NotMapped] public bool Selected { get; set; }
    [NotMapped] public IList<Question> Questions { get; set; }
}
