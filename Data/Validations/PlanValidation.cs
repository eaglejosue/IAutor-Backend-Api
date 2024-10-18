namespace IAutor.Api.Data.Validations;

public sealed class PlanCreateValidator : Validation<Plan>
{
    public PlanCreateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(500);
    }
}

public sealed class PlanUpdateValidator : Validation<Plan>
{
    public PlanUpdateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(500);
        RuleFor(p => p.Id).NotEmpty().NotNull();
    }
}

public sealed class PlanPatchValidator : Validation<Plan>
{
    public PlanPatchValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
    }
}


public static class PlanValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Plan u) => new PlanCreateValidator().ValidateCustomAsync(u);
    public static Task<ValidationResult> ValidateUpdateAsync(this Plan u) => new PlanUpdateValidator().ValidateCustomAsync(u);
    public static Task<ValidationResult> ValidatePatchAsync(this Plan u) => new PlanPatchValidator().ValidateCustomAsync(u);
}