namespace IAutor.Api.Services.External;

public interface IAiService
{
    Task<AiTextResponse> GenerateIAResponse(AiTextRequest request, long loggedUserId, string loggedUserName);
}

public class AiService(IConfiguration configuration) : IAiService
{
    public async Task<AiTextResponse> GenerateIAResponse(AiTextRequest request, long loggedUserId, string loggedUserName)
    {
        var msg = $"Eu {loggedUserName} ID {loggedUserId} estou escrevendo um livro sobre minha vida. Para a pergunta '{request.Question}' respondi: '{request.QuestionAnswer}'. ";
        msg += $"Melhore o conteúdo da resposta para adaptar ao livro com um contexto {request.Theme}. Limitando a resposta ao máximo de {request.MaxCaracters} caracteres. Retornar apenas conteúdo melhorado!";

        var key = configuration["ChatGpt:Key"];
        var client = new ChatClient("gpt-4o-mini", key);
        var completion = await client.CompleteChatAsync(msg);
        var response = completion.Value.Content[0].Text;

        return new AiTextResponse { Text = response };
    }
}
