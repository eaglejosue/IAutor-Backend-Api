namespace IAutor.Api.Data.Dtos.Iugu;

public sealed class IuguFaturaResponse : IuguResponse
{
    [JsonPropertyName("id")] public string FaturaId { get; set; }
    [JsonPropertyName("secure_id")] public string SecureId { get; set; }
    [JsonPropertyName("secure_url")] public string SecureUrl { get; set; }
    [JsonPropertyName("due_date")] public string DueDate { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; }
    [JsonPropertyName("paid")] public string Paid { get; set; }
}