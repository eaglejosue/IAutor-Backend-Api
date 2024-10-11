namespace IAutor.Api.Data.Dtos.Iugu;

public class IuguResponse
{
    [JsonPropertyName("errors")] public Dictionary<string, string> Errors { get; set; }
}

public class SubAccountVerificationResponse
{
    [JsonPropertyName("errors")] public SubAccountVerificationItemsResponse Errors { get; set; }
}

public class SubAccountVerificationItemsResponse
{
    [JsonPropertyName("agency")] public string[] Agency { get; set; }
    [JsonPropertyName("account")] public string[] Account { get; set; }
}