namespace IAutor.Api.Data.Entities;

public class Chapter : Base
{
    public string Title { get; set; }
    public string? ChapterNumber { get; set; }

    [JsonIgnore] public ICollection<Question>? Questions { get; set; }
}
