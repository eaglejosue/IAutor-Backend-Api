namespace IAutor.Api.Data.Validations;

public class ParamCreateValidator : Validation<Param>
{
    public ParamCreateValidator()
    {
        RuleFor(p => p.Key).NotEmpty().NotNull();
        RuleFor(p => p.Value).NotEmpty().NotNull();
    }
}

public sealed class ParamValidator : ParamCreateValidator
{
    public ParamValidator()
    {
        RuleFor(r => r.Key).NotNull().NotEmpty();
    }
}

public static class ParamValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Param e) => new ParamCreateValidator().ValidateCustomAsync(e);
    public static Task<ValidationResult> ValidateAsync(this Param e) => new ParamValidator().ValidateCustomAsync(e);
}