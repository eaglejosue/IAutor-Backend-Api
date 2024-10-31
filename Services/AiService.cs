using OpenAI;
using OpenAI.Chat;

namespace IAutor.Api.Services;

public interface IAiService
{
    Task<AiTextResponse> GenerateIaResponse(AiTextRequest request);

}
public class AiService : IAiService
{
    private readonly IConfiguration _configuration;
    public AiService(IConfiguration configuration) 
    { 
        _configuration = configuration; 
    }

    public async Task<AiTextResponse?> GenerateIaResponse(AiTextRequest request)
    {
        var key = _configuration["ChatGpt:Key"];
        ChatClient client = new(model: "gpt-4o-mini", apiKey: key);
        var charLimit = $"Limitando a resposta ao máximo de {request.maxCaracters} caracteres.";
        var theme = $"Escreva a resposta com um contexto {request.theme}.";
        ChatCompletion completion = client.CompleteChat($"{ request.questionResponse}, {theme}, {charLimit}");
        var response = completion.Content[0].Text;


        return new AiTextResponse() { TextResponse = response };
    }
}
