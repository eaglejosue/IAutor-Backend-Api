namespace Pay4Tru.Api.Data.Model;

public sealed class Video : Base
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
    [NotMapped] public long? CopyFromVideoId { get; set; }

    [NotMapped] public bool IsWatched => UserVideoLogs?.Count > 0;
    [NotMapped] public bool UserCanWatch => DateTimeBr.Now >= ReleaseDate;
    [NotMapped] public string? PaidDateTime {
        get
        {
            var payment = Orders?.FirstOrDefault()?.Payments?.FirstOrDefault();
            return payment?.IuguPaidAt ?? payment?.CreatedAt.ToString();
        }
    }

    public ICollection<VideoTrailer>? VideoTrailers { get; set; }
    public ICollection<OwnerVideo>? OwnerVideos { get; set; }
    public ICollection<Order>? Orders { get; set; }
    public ICollection<UserVideoLog>? UserVideoLogs { get; set; }
    public ICollection<Email>? Emails { get; set; }
}