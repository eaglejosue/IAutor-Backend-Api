namespace IAutor.Api.Data.Dtos.Iugu;

public sealed class IuguSubAccountVerificationRequest(Owner owner)
{
    [JsonPropertyName("data")] public IuguSubAccountVerificationDataRequest Data { get; set; } = new(owner);
}

public sealed class IuguSubAccountVerificationDataRequest(Owner owner)
{
    [JsonPropertyName("price_range")] public string PriceRange { get; set; } = "Até R$ 100,00";
    [JsonPropertyName("physical_products")] public bool PhysicalProducts { get; set; } = false;
    [JsonPropertyName("business_type")] public string BusinessType { get; set; } = "Venda de conteúdo digital.";
    [JsonPropertyName("person_type")] public string PersonType { get; set; } = owner.PersonType!;
    [JsonPropertyName("automatic_transfer")] public bool AutomaticTransfer { get; set; } = true;
    [JsonPropertyName("cnpj")] public string Cnpj { get; set; } = owner.Cnpj!;
    [JsonPropertyName("cpf")] public string Cpf { get; set; } = owner.Cpf!;
    [JsonPropertyName("company_name")] public string CompanyName { get; set; } = owner.PersonType! == Helpers.Constants.PersonType.PJ ? owner.Fullname : string.Empty;
    [JsonPropertyName("name")] public string Name { get; set; } = owner.PersonType! == Helpers.Constants.PersonType.PF ? owner.Fullname : string.Empty;
    [JsonPropertyName("address")] public string Address { get; set; } = owner.Address!;
    [JsonPropertyName("cep")] public string Cep { get; set; } = owner.Cep!;
    [JsonPropertyName("city")] public string City { get; set; } = owner.City!;
    [JsonPropertyName("district")] public string District { get; set; } = owner.District!;
    [JsonPropertyName("state")] public string State { get; set; } = owner.State!;
    [JsonPropertyName("telephone")] public string Telephone { get; set; } = owner.Telephone!;
    [JsonPropertyName("resp_name")] public string RespName { get; set; } = owner.CnpjRespName!;
    [JsonPropertyName("resp_cpf")] public string RespCpf { get; set; } = owner.CnpjRespCpf!;
    [JsonPropertyName("bank")] public string Bank { get; set; } = owner.Bank!;
    [JsonPropertyName("bank_ag")] public string BankAg { get; set; } = owner.BankAg!;
    [JsonPropertyName("agency")] public string Agency { get; set; } = owner.BankAg!;
    [JsonPropertyName("account_type")] public string AccountType { get; set; } = owner.BankAccountType!;
    [JsonPropertyName("bank_cc")] public string BankCc { get; set; } = owner.BankAccountNumber!;
}