namespace IAutor.Api.Data.Dtos.Iugu;

public sealed class IuguSubAccountCreateResponse : IuguResponse
{
    [JsonPropertyName("account_id")] public string AccountId { get; set; }
    [JsonPropertyName("live_api_token")] public string LiveApiToken { get; set; }
    [JsonPropertyName("test_api_token")] public string TestApiToken { get; set; }
    [JsonPropertyName("user_token")] public string UserToken { get; set; }
}