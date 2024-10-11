namespace Pay4Tru.Api.Data.Model;

public sealed class OwnerVideo
{
    public long Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long OwnerId { get; set; }
    public long VideoId { get; set; }
    public int PercentageSplit { get; set; }

    [NotMapped] public string IuguAccountId { get; set; }

    public Owner? Owner { get; set; }
    [JsonIgnore] public Video? Video { get; set; }

    internal void Inactivate()
    {
        IsActive = false;
        DeletedAt = DateTimeBr.Now;
    }
}