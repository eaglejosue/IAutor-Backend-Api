namespace IAutor.Api.Data.Validations;

public sealed class OrderCreateValidator : Validation<Order>
{
    public OrderCreateValidator()
    {
        RuleFor(p => p.UserId).NotEmpty().NotNull();
        RuleFor(p => p.VideoId).NotEmpty().NotNull();
    }
}

public sealed class OrderUpdateValidator : Validation<Order>
{
    public OrderUpdateValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
        RuleFor(p => p.UserId).NotEmpty().NotNull();
        RuleFor(p => p.VideoId).NotEmpty().NotNull();
    }
}

public sealed class OrderPatchValidator : Validation<Order>
{
    public OrderPatchValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
        RuleFor(p => p.UserId).NotEmpty().NotNull();
        RuleFor(p => p.VideoId).NotEmpty().NotNull();
    }
}

public static class OrderValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Order o) => new OrderCreateValidator().ValidateCustomAsync(o);
    public static Task<ValidationResult> ValidateUpdateAsync(this Order o) => new OrderUpdateValidator().ValidateCustomAsync(o);
    public static Task<ValidationResult> ValidatePatchAsync(this Order o) => new OrderPatchValidator().ValidateCustomAsync(o);
}