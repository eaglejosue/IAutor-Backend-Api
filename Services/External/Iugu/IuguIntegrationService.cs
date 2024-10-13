namespace IAutor.Api.Services.External.Iugu;

public interface IIuguIntegrationService
{
    Task<IuguFaturaResponse?> CreateFaturaAsync(string userEmail, long orderId, string title, decimal price);
    Task<IuguSubAccountCreateResponse?> CreateSubAccountAsync(string name);
    Task<SubAccountVerificationResponse?> VerifySubAccountAsync(Owner owner);
    Task<IuguAccountInfoResponse?> GetAccountInfoAsync(Owner owner);
    Task<IuguFinancialResponse?> GetFinancialBalanceByDayAsync(Owner owner, DateTime dateTime);
}

public sealed class IuguIntegrationService(
    IOptions<IuguConfig> iuguConfig,
    ILogger<WrapperHttpClient> logger,
    INotificationService notification,
    IConfiguration configuration) : IIuguIntegrationService
{
    private readonly WrapperHttpClient _wrapperHttpClient = new(logger, notification);

    public async Task<IuguFaturaResponse?> CreateFaturaAsync(string userEmail, long orderId, string title, decimal price)
    {
        var errorMsg = "Erro ao criar Fatura na IUGU.";

        try
        {
            var uri = string.Concat(iuguConfig.Value.UrlApi, iuguConfig.Value.EndpointCreateFatura);
            var urlPortalIAutor = configuration.GetValue<string>("Http:UrlPortalIAutor")!;

            var request = new IuguFaturaRequest(
                userEmail,
                orderId,
                title,
                price,
                iuguConfig.Value.QtdDiasVencimentoFatura,
                iuguConfig.Value.QtdDiasExpiracaoFatura,
                iuguConfig.Value.OpcoesPagamento,
                urlPortalIAutor);

#if DEBUG
            var requestJson = request.Serialize();
#endif

            var response = await _wrapperHttpClient.PostAsync<IuguFaturaResponse?>(
                uri,
                content: request.Serialize(),
                accessToken: iuguConfig.Value.Token.ToBase64(),
                authorizationType: AuthorizationType.Basic).ConfigureAwait(false);

            if (response == null)
                notification.AddNotification("Iugu", errorMsg);

            return response;
        }
        catch (Exception ex)
        {
            notification.AddNotification("Iugu", errorMsg);
            logger.LogError(ex, errorMsg);
            return default;
        }
    }

    public async Task<IuguSubAccountCreateResponse?> CreateSubAccountAsync(string name)
    {
        const string errorMsg = "Erro ao criar Subconta na IUGU.";

        try
        {
            var uri = string.Concat(iuguConfig.Value.UrlApi, iuguConfig.Value.EndpointCreateAccount);

            var response = await _wrapperHttpClient.PostAsync<IuguSubAccountCreateResponse?>(
                uri,
                content: new { name = name.NormalizeCustom() }.Serialize(),
                accessToken: iuguConfig.Value.Token.ToBase64(),
                authorizationType: AuthorizationType.Basic).ConfigureAwait(false);

            if (response == null)
                notification.AddNotification("Iugu", errorMsg);

            return response;
        }
        catch (Exception ex)
        {
            notification.AddNotification("Iugu", errorMsg);
            logger.LogError(ex, errorMsg);
            return default;
        }
    }

    public async Task<SubAccountVerificationResponse?> VerifySubAccountAsync(Owner owner)
    {
        const string errorMsg = "Erro ao verificar Subconta na IUGU.";

        try
        {
            var uri = string.Concat(iuguConfig.Value.UrlApi, iuguConfig.Value.EndpointVerifyAccount.Replace("{account_id}", owner.IuguAccountId));
            var request = new IuguSubAccountVerificationRequest(owner);

#if DEBUG
            var requestJson = request.Serialize();
#endif

            var response = await _wrapperHttpClient.PostAsync<SubAccountVerificationResponse>(
                uri,
                content: request.Serialize(),
                accessToken: owner.IuguUserToken.ToBase64(),
                authorizationType: AuthorizationType.Basic).ConfigureAwait(false);

            if (response == null)
                notification.AddNotification("Iugu", errorMsg);

            return response;
        }
        catch (Exception ex)
        {
            notification.AddNotification("Iugu", errorMsg);
            logger.LogError(ex, errorMsg);
            return default;
        }
    }

    public async Task<IuguAccountInfoResponse?> GetAccountInfoAsync(Owner owner)
    {
        const string errorMsg = "Erro ao obter informações da conta na IUGU.";

        try
        {
            var uri = string.Concat(iuguConfig.Value.UrlApi, iuguConfig.Value.EndpointAccountInfo.Replace("{account_id}", owner.IuguAccountId));

            var response = await _wrapperHttpClient.GetAsync<IuguAccountInfoResponse?>(
                uri,
                accessToken: owner.IuguUserToken.ToBase64(),
                authorizationType: AuthorizationType.Basic).ConfigureAwait(false);

            if (response == null)
                notification.AddNotification("Iugu", errorMsg);

            return response;
        }
        catch (Exception ex)
        {
            notification.AddNotification("Iugu", errorMsg);
            logger.LogError(ex, errorMsg);
            return default;
        }
    }

    public async Task<IuguFinancialResponse?> GetFinancialBalanceByDayAsync(Owner owner, DateTime dateTime)
    {
        const string errorMsg = "Erro ao obter extrato financeiro na IUGU.";

        try
        {
            var uri = string.Concat(iuguConfig.Value.UrlApi, iuguConfig.Value.EndpointFinancial,
                "?year=", dateTime.Year, "&month=", dateTime.Month, "&day=", dateTime.Day, "&limit=0");

            var response = await _wrapperHttpClient.GetAsync<IuguFinancialResponse?>(
                uri,
                accessToken: !string.IsNullOrEmpty(owner.IuguUserToken) ? owner.IuguUserToken.ToBase64() : iuguConfig.Value.Token.ToBase64(),
                authorizationType: AuthorizationType.Basic).ConfigureAwait(false);

            if (response == null)
                notification.AddNotification("Iugu", errorMsg);

            return response;
        }
        catch (Exception ex)
        {
            notification.AddNotification("Iugu", errorMsg);
            logger.LogError(ex, errorMsg);
            return default;
        }
    }
}
