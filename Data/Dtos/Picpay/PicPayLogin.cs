namespace IAutor.Api.Data.Dtos.Picpay
{
    public class PicPayLogin
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = "client_credentials";

        [JsonPropertyName("scopes")]
        public string Scopes { get; set; }
    }
}
