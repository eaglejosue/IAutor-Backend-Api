namespace IAutor.Api.Services.External;

public interface IAiService
{
    Task<AiTextResponse> GenerateIAResponse(AiTextRequest request, long loggedUserId, string loggedUserName);
}

public class AiService(IConfiguration configuration) : IAiService
{
    public async Task<AiTextResponse> GenerateIAResponse(AiTextRequest request, long loggedUserId, string loggedUserName)
    {
        var msg = $"Eu {loggedUserName} estou escrevendo um book com ID {request.BookId} sobre minha vida. Para a pergunta '{request.Question}' respondi: '{request.QuestionAnswer}'. ";
        msg += $"Melhore a resposta para adaptar ao book com um contexto {request.Theme}. Limitando a resposta ao máximo de {request.MaxCaracters} caracteres.";

        var key = configuration["ChatGpt:Key"];
        var client = new ChatClient("gpt-4o-mini", key);
        var completion = await client.CompleteChatAsync(msg);
        var response = completion.Value.Content[0].Text;

        return new AiTextResponse { Text = response };
    }
}
