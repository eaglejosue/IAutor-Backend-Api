namespace IAutor.Api.Data.Entities;

public sealed class Book : Base
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? PublicId { get; set; }
    public string? ThumbImgUrl { get; set; }
    public DateTime? SaleExpirationDate { get; set; }
    public DateTime? DownloadExpirationDate { get; set; }
    public decimal? PromotionPrice { get; set; }
    public DateTime? PromotionExpirationDate { get; set; }
    public long PlanId { get; set; }
    public long UserId { get; set; }
    public BookType? Type { get; set; }

    public Plan Plan { get; set; }
    [JsonIgnore] public User User { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public ICollection<UserBookLog>? UserBookLogs { get; set; }
    public ICollection<Email>? Emails { get; set; }
    public ICollection<QuestionUserAnswer>? QuestionUserAnswers { get; set; }

    [NotMapped] public string? PaidDateTime
    {
        get
        {
            var payment = Orders?.FirstOrDefault()?.Payments?.FirstOrDefault();
            return payment?.IuguPaidAt ?? payment?.CreatedAt.ToString();
        }
    }
}