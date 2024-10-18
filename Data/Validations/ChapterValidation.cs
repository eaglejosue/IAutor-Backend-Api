namespace IAutor.Api.Data.Validations;

public sealed class ChapterCreateValidator : Validation<Chapter>
{
    public ChapterCreateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(500);
        RuleFor(p => p.ChapterNumber).NotEmpty().NotNull();
    }
}

public sealed class ChapterUpdateValidator : Validation<Chapter>
{
    public ChapterUpdateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(500);
        RuleFor(p => p.ChapterNumber).NotEmpty().NotNull();
        RuleFor(p => p.Id).NotEmpty().NotNull();
    }
}

public sealed class ChapterPatchValidator : Validation<Chapter>
{
    public ChapterPatchValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
    }
}


public static class ChapterValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Chapter u) => new ChapterCreateValidator().ValidateCustomAsync(u);
    public static Task<ValidationResult> ValidateUpdateAsync(this Chapter u) => new ChapterUpdateValidator().ValidateCustomAsync(u);
    public static Task<ValidationResult> ValidatePatchAsync(this Chapter u) => new ChapterPatchValidator().ValidateCustomAsync(u);
}