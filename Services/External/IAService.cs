namespace IAutor.Api.Services.External;

public interface IIAService
{
    Task<AiTextResponse> GenerateIAResponse(AiTextRequest request, long loggedUserId, string loggedUserName);
}

public class IAService(IConfiguration configuration) : IIAService
{
    public async Task<AiTextResponse> GenerateIAResponse(AiTextRequest request, long loggedUserId, string loggedUserName)
    {
        var msgBuilder = new StringBuilder($"Eu {loggedUserName} ID {loggedUserId} estou escrevendo um livro sobre minha vida, BOOK_ID {request.BookId}. ");
        msgBuilder.Append($"Para a pergunta '{request.Question}' respondi: '{request.QuestionAnswer}'. ");
        msgBuilder.Append($"Melhore o conteúdo da resposta para adaptar ao livro com um contexto {request.Theme}. ");
        msgBuilder.Append($"Limitando a resposta ao máximo de {request.MaxCaracters} caracteres. Retornar apenas conteúdo melhorado!");

        var key = configuration["ChatGpt:Key"];
        var client = new ChatClient("gpt-4o-mini", key);
        var completion = await client.CompleteChatAsync(msgBuilder.ToString());
        var response = completion.Value.Content[0].Text;

        return new AiTextResponse { Text = response };
    }
}
