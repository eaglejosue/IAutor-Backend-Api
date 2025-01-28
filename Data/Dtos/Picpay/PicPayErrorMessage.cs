namespace IAutor.Api.Data.Dtos.Picpay
{
    public class PicPayErrorMessage
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("businessCode")]
        public string BusinessCode { get; set; }
      
        public List<ErrorPicPay> Errors { get; set; }

    }

    public class ErrorPicPay
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("field")]
        public string Field { get; set; }
    }
}
