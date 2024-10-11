namespace Pay4Tru.Api.Data.Model;

public sealed class Income
{
    public long Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long OwnerId { get; set; }
    public DateTime DateReference { get; set; }
    public decimal SumValue { get; set; }
    public long SalesAmount { get; set; }

    public Owner? Owner { get; set; }
}