namespace IAutor.Api.Data.Validations;

public sealed class IncomeCreateValidator : Validation<Income>
{
    public IncomeCreateValidator()
    {
        RuleFor(p => p.OwnerId).NotEmpty().NotNull();
        RuleFor(p => p.DateReference).NotEmpty().NotNull();
        RuleFor(p => p.SumValue).NotEmpty().NotNull();
    }
}

public sealed class IncomeUpdateValidator : Validation<Income>
{
    public IncomeUpdateValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
        RuleFor(p => p.OwnerId).NotEmpty().NotNull();
        RuleFor(p => p.DateReference).NotEmpty().NotNull();
        RuleFor(p => p.SumValue).NotEmpty().NotNull();
    }
}

public sealed class IncomePatchValidator : Validation<Income>
{
    public IncomePatchValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
        RuleFor(p => p.OwnerId).NotEmpty().NotNull();
    }
}

public static class IncomeValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Income p) => new IncomeCreateValidator().ValidateCustomAsync(p);
    public static Task<ValidationResult> ValidateUpdateAsync(this Income p) => new IncomeUpdateValidator().ValidateCustomAsync(p);
    public static Task<ValidationResult> ValidatePatchAsync(this Income p) => new IncomePatchValidator().ValidateCustomAsync(p);
}