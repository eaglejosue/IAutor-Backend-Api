namespace IAutor.Api.Data.Validations;

public sealed class BookCreateValidator : Validation<Book>
{
    public BookCreateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(80);
        RuleFor(p => p.Description).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(p => p.Price).NotEmpty().NotNull().GreaterThan(0.99M).WithMessage("Preço deve ser no mínimo R$ 1,00.");
        RuleFor(p => p.PlanId).NotEmpty().NotNull();
        RuleFor(p => p.UserId).NotEmpty().NotNull();
    }
}

public sealed class BookUpdateValidator : Validation<Book>
{
    public BookUpdateValidator()
    {
        RuleFor(p => p.Id).NotNull().NotEmpty();
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(80);
        RuleFor(p => p.Description).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(p => p.Price).NotEmpty().NotNull().GreaterThan(0.99M).WithMessage("Preço deve ser no mínimo R$ 1,00.");
        RuleFor(p => p.PlanId).NotEmpty().NotNull();
        RuleFor(p => p.UserId).NotEmpty().NotNull();
    }
}

public sealed class BookPatchValidator : Validation<Book>
{
    public BookPatchValidator()
    {
        RuleFor(p => p.Id).NotNull().NotEmpty();
    }
}

public static class BookValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Book v) => new BookCreateValidator().ValidateCustomAsync(v);
    public static Task<ValidationResult> ValidateUpdateAsync(this Book v) => new BookUpdateValidator().ValidateCustomAsync(v);
    public static Task<ValidationResult> ValidatePatchAsync(this Book v) => new BookPatchValidator().ValidateCustomAsync(v);
}