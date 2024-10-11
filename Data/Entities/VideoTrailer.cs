namespace Pay4Tru.Api.Data.Model;

public sealed class VideoTrailer
{
    public long Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string Title { get; set; }
    public long VideoId { get; set; }
    public string CloudinaryPublicId { get; set; }

    [JsonIgnore] public Video? Video { get; set; }

    internal void Inactivate()
    {
        IsActive = false;
        DeletedAt = DateTimeBr.Now;
    }
}