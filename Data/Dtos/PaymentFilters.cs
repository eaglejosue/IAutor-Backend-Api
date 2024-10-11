namespace IAutor.Api.Data.Dtos;

public sealed class PaymentFilters : BaseFilters
{
    public long? OrderId { get; set; }
    public decimal? PricePaid { get; set; }
    public PaymentStatusEnum? PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? Filter { get; set; }
}
