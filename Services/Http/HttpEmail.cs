namespace IAutor.Api.Services.Http;

public interface IHttpEmail
{
    Task SendActivateAccountAsync(long emailId);
    Task SendForgotPasswordAsync(long emailId);
    Task SendVideoReleaseAsync(long emailId);
}

public class HttpEmail(ILogger<WrapperHttpClient> logger, INotificationService notification, IConfiguration configuration) : IHttpEmail
{
    private readonly WrapperHttpClient _wrapperHttpClient = new(logger, notification);
    private readonly string _urlEmail = configuration.GetValue<string>("Http:UrlApiEmail")!;

    public async Task SendActivateAccountAsync(long emailId)
    {
        try
        {
            await _wrapperHttpClient.PostAsync(string.Concat(_urlEmail, "/api/emails/send-activate-account/", emailId)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Concat("Erro ao enviar E-mail de ativação. Email Id: ", emailId));
        }
    }

    public async Task SendForgotPasswordAsync(long emailId)
    {
        try
        {
            await _wrapperHttpClient.PostAsync(string.Concat(_urlEmail, "/api/emails/send-forgot-password/", emailId)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Concat("Erro ao enviar E-mail de Recuperação de Senha. Email Id: ", emailId));
        }
    }

    public async Task SendVideoReleaseAsync(long emailId)
    {
        try
        {
            await _wrapperHttpClient.PostAsync(string.Concat(_urlEmail, "/api/emails/send-video-release/", emailId)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Concat("Erro ao enviar E-mail de Notificação de Lançamento de Vídeo. Id: ", emailId));
        }
    }
}
