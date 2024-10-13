namespace IAutor.Api.Data.Dtos;

public sealed class UserFilters : BaseFilters
{
    public string? Filter { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? SignInWith { get; set; }
    public UserTypeEnum? Type { get; set; }
    public DateTime? BirthDate { get; set; }
    public long? BookId { get; set; }
}