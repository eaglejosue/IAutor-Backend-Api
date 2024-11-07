namespace IAutor.Api.Services.External;

public interface IAiService
{
    Task<AiTextResponse> GenerateIAResponse(AiTextRequest request);
}

public class AiService(IConfiguration configuration) : IAiService
{
    public async Task<AiTextResponse> GenerateIAResponse(AiTextRequest request)
    {
        var msg = $"Estou escrevendo um book com ID {request.BookId} sobre da vida de {request.UserName}. Perguntei para o usuário '{request.Question}' e recebi a resposta: '{request.QuestionResponse}'. ";
        msg += $"Melhore a resposta para adaptar ao book com um contexto {request.Theme}. Limitando a resposta ao máximo de {request.MaxCaracters} caracteres.";

        var key = configuration["ChatGpt:Key"];
        var client = new ChatClient("gpt-4o-mini", key);
        var completion = await client.CompleteChatAsync(msg);
        var response = completion.Value.Content[0].Text;

        return new AiTextResponse { TextResponse = response };
    }
}
