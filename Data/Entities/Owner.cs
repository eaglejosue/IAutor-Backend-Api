namespace IAutor.Api.Data.Entities;

public sealed class Owner : Base
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? SocialUserName { get; set; }
    public OwnerTypeEnum? Type { get; set; }
    public long UserId { get; set; }
    public string? ProfileImgUrl { get; set; }
    public string? Instagram { get; set; }
    public string? TikTok { get; set; }
    public string? PersonType { get; set; }//'Pessoa Física' ou 'Pessoa Jurídica'
    public string? Cpf { get; set; }
    public string? Cnpj { get; set; }
    public string? CnpjRespName { get; set; }//Nome do responsável caso CNPJ
    public string? CnpjRespCpf { get; set; }//CPF do responsável caso CNPJ
    public string? Address { get; set; }
    public string? Cep { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? State { get; set; }
    public string? Telephone { get; set; }
    public string? Bank { get; set; }
    public string? BankAg { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankAccountType { get; set; }
    public bool? IuguAccountVerified { get; set; }
    public string? IuguAccountId { get; set; }
    public string? IuguUserToken { get; set; }
    public string? IuguLiveApiToken { get; set; }
    public string? IuguTestApiToken { get; set; }

    [NotMapped] public string? Email { get; set; }
    [NotMapped] public string? Password { get; set; }
    [NotMapped] public string Fullname => string.Concat(FirstName ?? string.Empty, " ", LastName ?? string.Empty);
    [NotMapped] public string? Doc => !string.IsNullOrEmpty(Cpf) ? Cpf : Cnpj;

    public User? User { get; set; }
    [JsonIgnore] public ICollection<Income>? Incomes { get; set; }

    public void SetIuguTokens(IuguSubAccountCreateResponse subAccount)
    {
        IuguAccountVerified = false;
        IuguAccountId = subAccount.AccountId;
        IuguUserToken = subAccount.UserToken;
        IuguLiveApiToken = subAccount.LiveApiToken;
        IuguTestApiToken = subAccount.TestApiToken;
    }
}

public sealed class SocialMedia
{
    public string Media { get; set; }
    public string Url { get; set; }
}