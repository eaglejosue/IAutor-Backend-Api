namespace IAutor.Api.Data.Dtos;

public class AiTextResponse
{
    public string TextResponse { get; set; }    
}
public class AiTextRequest
{
    public string questionResponse { get; set; }

    public string theme { get; set; }

    public int maxCaracters { get; set; }

    public long? bookId { get; set; }
    public long? questionId { get; set; }
    public long? chapterId { get; set; }

}