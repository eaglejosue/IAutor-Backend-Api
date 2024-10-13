namespace IAutor.Api.Data.Dtos;

public sealed class BookFilters : BaseFilters
{
    public string? Filter { get; set; }
    public string? CloudinaryPublicId { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public decimal? Price { get; set; }
    public DateTime? SaleExpirationDate { get; set; }
    public decimal? PromotionPrice { get; set; }
    public DateTime? PromotionExpirationDate { get; set; }
    public long? TrailerId { get; set; }
    public bool? IncludeTrailers { get; set; }
    public bool? PaymentsApproved { get; set; }
    public bool? IncludePayments { get; set; }
    public bool? IncludeUserBookLogs { get; set; }
    public bool? ListToCrud { get; set; }
    public bool? ListToWatch { get; set; }
}
