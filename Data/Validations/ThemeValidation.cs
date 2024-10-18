namespace IAutor.Api.Data.Validations;

public sealed class ThemeCreateValidator : Validation<Theme>
{
    public ThemeCreateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(500);
    }
}

public sealed class ThemeUpdateValidator : Validation<Theme>
{
    public ThemeUpdateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(500);
        RuleFor(p => p.Id).NotEmpty().NotNull();
    }
}

public sealed class ThemePatchValidator : Validation<Theme>
{
    public ThemePatchValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
    }
}


public static class ThemeValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Theme u) => new ThemeCreateValidator().ValidateCustomAsync(u);
    public static Task<ValidationResult> ValidateUpdateAsync(this Theme u) => new ThemeUpdateValidator().ValidateCustomAsync(u);
    public static Task<ValidationResult> ValidatePatchAsync(this Theme u) => new ThemePatchValidator().ValidateCustomAsync(u);
}