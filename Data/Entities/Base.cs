namespace Pay4Tru.Api.Data.Model;

public class Base
{
    public long Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Base()
    {
        IsActive = true;
        CreatedAt = DateTimeBr.Now;
    }
};