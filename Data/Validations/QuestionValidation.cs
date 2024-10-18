namespace IAutor.Api.Data.Validations;

public sealed class QuestionCreateValidator : Validation<Question>
{
    public QuestionCreateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(500);
    }
}

public sealed class QuestionUpdateValidator : Validation<Question>
{
    public QuestionUpdateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(500);
        RuleFor(p => p.Id).NotEmpty().NotNull();
    }
}

public sealed class QuestionPatchValidator : Validation<Question>
{
    public QuestionPatchValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
    }
}


public static class QuestionValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Question u) => new QuestionCreateValidator().ValidateCustomAsync(u);
    public static Task<ValidationResult> ValidateUpdateAsync(this Question u) => new QuestionUpdateValidator().ValidateCustomAsync(u);
    public static Task<ValidationResult> ValidatePatchAsync(this Question u) => new QuestionPatchValidator().ValidateCustomAsync(u);
}