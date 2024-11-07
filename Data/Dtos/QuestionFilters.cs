namespace IAutor.Api.Data.Dtos;

public sealed class QuestionFilters : BaseFilters
{
    public string? Title { get; set; }

    public long? ChapterId { get; set; }

    public string? Subject { get; set; }

}