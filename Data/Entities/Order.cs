namespace IAutor.Api.Data.Entities;

public sealed class Order : Base
{
    public long UserId { get; set; }
    public long BookId { get; set; }

    [NotMapped] public string? IuguFaturaSecureUrl { get; set; }
    [NotMapped] public long PlanId { get; set; }

    [JsonIgnore] public User? User { get; set; }
    [JsonIgnore] public Book? Book { get; set; }
    public ICollection<Payment>? Payments { get; set; }
}