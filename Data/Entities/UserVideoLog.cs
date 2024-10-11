namespace Pay4Tru.Api.Data.Model;

public sealed class UserVideoLog
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
    public long VideoId { get; set; }
    public string Log { get; set; }

    [JsonIgnore] public User? User { get; set; }
    [JsonIgnore] public Video? Video { get; set; }
}