namespace IAutor.Api.Data.Dtos.Iugu;

public sealed class IuguPaymentChangedStatus
{
    public string? Event { get; set; }
    public string? FaturaId { get; set; }
    public string? AccountId { get; set; }
    public string? Status { get; set; }
    public string? PaymentMethod { get; set; }
    public string? PaidAt { get; set; }
    public string? PayerCpfCnpj { get; set; }
    public string? PixEndToEndId { get; set; }
    public string? PaidCents { get; set; }
    public string? OrderId { get; set; }
    public string? ExternalReference { get; set; }

    public static async ValueTask<IuguPaymentChangedStatus?> BindAsync(HttpContext httpContext, ParameterInfo _)
    {
        var form = await httpContext.Request.ReadFormAsync();

        return new IuguPaymentChangedStatus
        {
            Event = form["event"],
            FaturaId = form["data[id]"],
            AccountId = form["data[account_id]"],
            Status = form["data[status]"],
            PaymentMethod = form["data[payment_method]"],
            PaidAt = form["data[paid_at]"],
            PayerCpfCnpj = form["data[payer_cpf_cnpj]"],
            PixEndToEndId = form["data[pix_end_to_end_id]"],
            PaidCents = form["data[paid_cents]"],
            OrderId = form["data[order_id]"],
            ExternalReference = form["data[external_reference]"]
        };
    }
}