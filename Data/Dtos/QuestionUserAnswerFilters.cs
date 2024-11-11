namespace IAutor.Api.Data.Dtos;

public sealed class QuestionUserAnswerFilters : BaseFilters
{
    public long? QuestionId { get; set; }
    public long? UserId { get; set; }
}