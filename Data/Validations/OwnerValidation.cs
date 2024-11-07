namespace IAutor.Api.Data.Validations;

public sealed class OwnerCreateValidator : Validation<Owner>
{
    public OwnerCreateValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().NotNull();
        RuleFor(p => p.LastName).NotEmpty().NotNull();
        RuleFor(p => p.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(p => p.Password).NotEmpty().NotNull();
        RuleFor(p => p.PersonType).Must(p => p == PersonType.PF || p == PersonType.PJ).NotEmpty().NotNull();
        When(p => p.PersonType == PersonType.PF, () =>
        {
            RuleFor(p => p.Cpf).NotEmpty().NotNull();
        });
        When(p => p.PersonType == PersonType.PJ, () =>
        {
            RuleFor(p => p.Cnpj).NotEmpty().NotNull();
            RuleFor(p => p.CnpjRespCpf).NotEmpty().NotNull();
            RuleFor(p => p.CnpjRespName).NotEmpty().NotNull();
        });
        RuleFor(p => p.Address).NotEmpty().NotNull();
        RuleFor(p => p.Cep).NotEmpty().NotNull();
        RuleFor(p => p.City).NotEmpty().NotNull();
        RuleFor(p => p.District).NotEmpty().NotNull();
        RuleFor(p => p.State).NotEmpty().NotNull();
        RuleFor(p => p.Telephone).NotEmpty().NotNull();
        RuleFor(p => p.Bank).NotEmpty().NotNull();
        RuleFor(p => p.BankAg).NotEmpty().NotNull();
        RuleFor(p => p.BankAccountType).NotEmpty().NotNull();
        RuleFor(p => p.BankAccountNumber).NotEmpty().NotNull();
    }
}

public sealed class OwnerUpdateValidator : Validation<Owner>
{
    public OwnerUpdateValidator()
    {
        RuleFor(p => p.Id).NotNull().NotEmpty();
        RuleFor(p => p.FirstName).NotEmpty().NotNull();
        RuleFor(p => p.LastName).NotEmpty().NotNull();
    }
}

public sealed class OwnerPatchValidator : Validation<Owner>
{
    public OwnerPatchValidator()
    {
        RuleFor(r => r.Id).NotNull().NotEmpty();
    }
}

public static class OwnerValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Owner o) => new OwnerCreateValidator().ValidateCustomAsync(o);
    public static Task<ValidationResult> ValidateUpdateAsync(this Owner o) => new OwnerUpdateValidator().ValidateCustomAsync(o);
    public static Task<ValidationResult> ValidatePatchAsync(this Owner o) => new OwnerPatchValidator().ValidateCustomAsync(o);
}