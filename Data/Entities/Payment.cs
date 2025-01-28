namespace IAutor.Api.Data.Entities;

public sealed class Payment : Base
{
    public long OrderId { get; set; }
    public decimal PricePaid { get; set; }
    public PaymentStatus? Status { get; set; }
    public string? IuguEvent { get; set; }
    public string? IuguFaturaId { get; set; }
    public string? IuguOrderId { get; set; }
    public string? IuguAccountId { get; set; }
    public string? IuguStatus { get; set; }
    public string? IuguExternalReference { get; set; }
    public string? IuguPaymentMethod { get; set; }
    public string? IuguPaidAt { get; set; }
    public string? IuguPayerCpfCnpj { get; set; }
    public string? IuguPixEndToEndId { get; set; }
    public string? IuguPaidCents { get; set; }
    public string? IuguJsonResult { get; set; }

    [JsonIgnore] public Order? Order { get; set; }

    public Payment() { }

    public Payment(Order o, decimal pricePaid, IuguFaturaResponse? fatura = null, PaymentStatus? status = PaymentStatus.Pending)
    {
        OrderId = o.Id;
        CreatedAt = DateTimeBr.Now;
        PricePaid = pricePaid;
        Status = status;
        IuguFaturaId = fatura?.FaturaId;
        IuguOrderId = o.Id.ToString();
        IuguStatus = fatura?.Status;
    }

    public Payment(Payment p)
    {
        CreatedAt = DateTimeBr.Now;
        OrderId = p.OrderId;
        PricePaid = p.PricePaid;
        Status = p.Status;
        IuguFaturaId = p.IuguFaturaId;
        IuguOrderId = p.IuguOrderId;
    }

    public void SetIuguData(IuguPaymentChangedStatus model)
    {
        Status = _dictionaryStatus[model.Status!];
        IuguEvent = model.Event;
        IuguStatus = model.Status;
        IuguExternalReference = model.ExternalReference;
        IuguPaymentMethod = model.PaymentMethod;
        IuguPaidAt = model.PaidAt;
        IuguPayerCpfCnpj = model.PayerCpfCnpj;
        IuguPixEndToEndId = model.PixEndToEndId;
        IuguPaidCents = model.PaidCents;
        IuguJsonResult = JsonSerializer.Serialize(model);
    }

    private static readonly FrozenDictionary<string, PaymentStatus> _dictionaryStatus = new Dictionary<string, PaymentStatus>
    {
        { "pending", PaymentStatus.Pending },
        { "paid", PaymentStatus.Paid },
        { "canceled", PaymentStatus.Canceled },
        { "partially_paid", PaymentStatus.PartiallyPaid },
        { "refunded", PaymentStatus.Refunded },
        { "expired", PaymentStatus.Expired },
        { "authorized", PaymentStatus.Authorized },
        { "externally_paid", PaymentStatus.ExternallyPaid },
        { "in_protest", PaymentStatus.InProtest },
        { "chargeback", PaymentStatus.Chargeback }
    }.ToFrozenDictionary();
}