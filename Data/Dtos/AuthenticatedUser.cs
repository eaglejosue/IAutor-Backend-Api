namespace IAutor.Api.Data.Dtos;

public record AuthenticatedUser(
    long Id,
    int Type,
    string Name,
    string Firstname,
    string Lastname,
    string Email,
    string? ProfileImgUrl,
    long? OwnerId,
    bool IsValid,
    string Token
);