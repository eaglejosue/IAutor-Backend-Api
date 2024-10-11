namespace IAutor.Api.Data.Dtos;

public sealed class OrderFilters : BaseFilters
{
    public long? UserId { get; set; }
    public bool? IncludeUser { get; set; }
    public long? VideoId { get; set; }
    public bool? IncludeVideo { get; set; }
    public long? PaymentId { get; set; }
    public bool? IncludePayment { get; set; }
}
