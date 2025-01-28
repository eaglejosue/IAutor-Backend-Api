namespace IAutor.Api.Data.Dtos.Picpay
{
    public class PicPayCheckoutResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("checkoutUrl")]
        public string CheckOutUrl { get; set; }
    }
}
