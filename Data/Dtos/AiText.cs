namespace IAutor.Api.Data.Dtos;

public class AiTextResponse
{
    public string TextResponse { get; set; }    
}

public class AiTextRequest
{
    public required string Question { get; set; }
    public required string QuestionResponse { get; set; }
    public required string Theme { get; set; }
    public int MaxCaracters { get; set; }

    public long? UserId { get; set; }
    public required string UserName { get; set; }
    public long? BookId { get; set; }
    public long? ChapterId { get; set; }
    public long? QuestionId { get; set; }
}