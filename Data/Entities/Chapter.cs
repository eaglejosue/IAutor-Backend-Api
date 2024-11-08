namespace IAutor.Api.Data.Entities;

public class Chapter : Base
{
    public string Title { get; set; }
    public int? ChapterNumber { get; set; }

    [JsonIgnore] public ICollection<PlanChapter>? PlansChapters { get; }

    [NotMapped] public bool Selected { get; set; }
}
