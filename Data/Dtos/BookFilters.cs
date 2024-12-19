namespace IAutor.Api.Data.Dtos;

public sealed class BookFilters : BaseFilters
{
    public string? Filter { get; set; }
    public string? PublicId { get; set; }
    public decimal? Price { get; set; }
    public DateTime? SaleExpirationDate { get; set; }
    public decimal? PromotionPrice { get; set; }
    public DateTime? PromotionExpirationDate { get; set; }
    public bool? PaymentsApproved { get; set; }
    public bool? IncludePayments { get; set; }
    public bool? IncludeUserBookLogs { get; set; }
    public bool? IncludeUserBookPlan { get; set; }
    public bool? IncludeQuestionUserAnswers { get; set; }
    public bool? ListToCrud { get; set; }
    public bool? ListToDownload { get; set; }
    public long? PlanId { get; set; }
    public long? UserId { get; set; }
}
