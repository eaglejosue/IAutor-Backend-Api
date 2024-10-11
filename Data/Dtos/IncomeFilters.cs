namespace IAutor.Api.Data.Dtos;

public sealed class IncomeFilters : BaseFilters
{
    public long? OwnerId { get; set; }
    public bool? IncludeOwner { get; set; }
    public DateTime? DateReference { get; set; }
    public decimal? SumValue { get; set; }
}

public sealed class IncomeGroupedFilters
{
    public long? OwnerId { get; set; }
    public string? Filter { get; set; }
    public int? QtdDays { get; set; } = 7;
}
