namespace IAutor.Api.Data.Dtos;

public sealed class IncomeGroupedByDayDto
{
    public long OwnerId { get; set; }
    public string OwnerProfileImgUrl { get; set; }
    public string OwnerName { get; set; }
    public OwnerTypeEnum? OwnerType { get; set; }
    public List<string> Dates { get; set; } = [];
    public bool ValueIsMoney { get; set; }
    public bool ValueAcomulated { get; set; }
    public List<decimal> Values { get; set; } = [];
    public List<long> QtdValues { get; set; } = [];
    public decimal Total { get; set; }
    public long QtdTotal { get; set; }
    public string IuguBalance { get; set; }
}
