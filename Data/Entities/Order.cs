namespace Pay4Tru.Api.Data.Model;

public sealed class Order : Base
{
    public long UserId { get; set; }
    public long VideoId { get; set; }

    [NotMapped] public string? IuguFaturaSecureUrl { get; set; }

    [JsonIgnore] public User? User { get; set; }
    [JsonIgnore] public Video? Video { get; set; }
    public ICollection<Payment>? Payments { get; set; }
}