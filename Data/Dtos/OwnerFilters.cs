namespace IAutor.Api.Data.Dtos;

public sealed class OwnerFilters : BaseFilters
{
    public string? Filter { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool? IncludeUserInfo { get; set; }
    public bool? IuguAccountVerified { get; set; }
    public bool? GetIdAndNameOnly { get; set; }
}
