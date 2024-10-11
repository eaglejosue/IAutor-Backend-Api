namespace IAutor.Api.Data.Validations;

public sealed class PaymentCreateValidator : Validation<Payment>
{
    public PaymentCreateValidator()
    {
        RuleFor(p => p.OrderId).NotEmpty().NotNull();
        RuleFor(p => p.PricePaid).NotEmpty().NotNull();
    }
}

public sealed class PaymentUpdateValidator : Validation<Payment>
{
    public PaymentUpdateValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
        RuleFor(p => p.OrderId).NotEmpty().NotNull();
        RuleFor(p => p.PricePaid).NotEmpty().NotNull();
    }
}

public sealed class PaymentPatchValidator : Validation<Payment>
{
    public PaymentPatchValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
        RuleFor(p => p.OrderId).NotEmpty().NotNull();
    }
}

public static class PaymentValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Payment p) => new PaymentCreateValidator().ValidateCustomAsync(p);
    public static Task<ValidationResult> ValidateUpdateAsync(this Payment p) => new PaymentUpdateValidator().ValidateCustomAsync(p);
    public static Task<ValidationResult> ValidatePatchAsync(this Payment p) => new PaymentPatchValidator().ValidateCustomAsync(p);
}