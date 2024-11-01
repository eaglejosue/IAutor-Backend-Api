namespace IAutor.Api.Data.Entities;

public sealed class Question : Base
{
    public string Title { get; set; }
    public int MaxLimitCharacters { get; set; } = 10000;
    public int MinLimitCharacters { get; set; } = 1;
    public long ChapterId { get; set; }

    [JsonIgnore] public Chapter Chapter { get; set; }
}
