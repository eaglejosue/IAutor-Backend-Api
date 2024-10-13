namespace IAutor.Api.Data.Entities;

public sealed class Book : Base
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Price { get; set; }
    public string CloudinaryPublicId { get; set; }
    public string? ThumbImgUrl { get; set; }
    public DateTime? SaleExpirationDate { get; set; }
    public DateTime? WatchExpirationDate { get; set; }
    public decimal? PromotionPrice { get; set; }
    public DateTime? PromotionExpirationDate { get; set; }
    public string? UpdatedBy { get; set; }

    [NotMapped] public string? PaidDateTime {
        get
        {
            var payment = Orders?.FirstOrDefault()?.Payments?.FirstOrDefault();
            return payment?.IuguPaidAt ?? payment?.CreatedAt.ToString();
        }
    }

    public ICollection<BookDegust>? BookDegusts { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public ICollection<UserBookLog>? UserBookLogs { get; set; }
    public ICollection<Email>? Emails { get; set; }
}