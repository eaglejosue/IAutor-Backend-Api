namespace IAutor.Api.Data.Dtos.Iugu;

public sealed class IuguConfig
{
    public required string Token { get; set; }
    public required string UrlApi { get; init; }
    public required string EndpointCreateFatura { get; init; }
    public required string EndpointCreateAccount { get; init; }
    public required string EndpointVerifyAccount { get; init; }
    public required string EndpointAccountInfo { get; init; }
    public required string EndpointFinancial { get; init; }
    public required int QtdDiasVencimentoFatura { get; set; }
    public required int QtdDiasExpiracaoFatura { get; set; }
    public required string[] OpcoesPagamento { get; set; }
}
