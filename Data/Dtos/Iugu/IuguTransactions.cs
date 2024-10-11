namespace IAutor.Api.Data.Dtos.Iugu;

public sealed class IuguTransactionsRequest : IuguResponse
{
    [JsonPropertyName("id")] public string FaturaId { get; set; }
}

public sealed class IuguTransactionsResponse : IuguResponse
{
    [JsonPropertyName("id")] public string FaturaId { get; set; }
}