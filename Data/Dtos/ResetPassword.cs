namespace IAutor.Api.Data.Dtos;

public record ResetPassword(
    Guid? ResetPasswordCode,
    string? NewPassword
);
