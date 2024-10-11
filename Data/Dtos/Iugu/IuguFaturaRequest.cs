namespace IAutor.Api.Data.Dtos.Iugu;

public sealed class IuguFaturaRequest(
    string userEmail,
    long orderId,
    string videoTitulo,
    decimal price,
    List<OwnerVideo> ownerVideos,
    int qtdDiasVencimentoFatura,
    int qtdDiasExpiracaoFatura,
    string[] payableWith,
    string urlPortalIAutor)
{
    [JsonPropertyName("email")]//E-mail do cliente
    public string Email { get; set; } = userEmail;
    [JsonPropertyName("due_date")]//Data do vencimento. (Formato: 'AAAA-MM-DD')
    public string DueDate { get; set; } = DateTimeBr.Date.AddDays(qtdDiasVencimentoFatura).ToString("yyyy-MM-dd");
    [JsonPropertyName("order_id")]
    public string OrderId { get; set; } = orderId.ToString();
    [JsonPropertyName("expires_in")]//Expira uma fatura e impossibilita o seu pagamento depois 'x' dias após o vencimento
    public string ExpiresIn { get; set; } = qtdDiasExpiracaoFatura.ToString();
    [JsonPropertyName("items")]
    public List<IuguFaturaItem> Items { get; set; } = [new(string.Concat("Vídeo: ", videoTitulo), 1, Convert.ToInt32(price * 100))];
    [JsonPropertyName("return_url")]//Url para onde usuário será direcionado ao finalizar pagamento
    public string ReturnUrl { get; set; } = string.Concat(urlPortalIAutor, "/paid-truths?paid=true");
    [JsonPropertyName("splits")]//Split de valor para owners do vídeo
    public List<IuguFaturaSplit> Splits { get; set; } = ownerVideos.Select(s => new IuguFaturaSplit(s.IuguAccountId, s.PercentageSplit)).ToList();
    [JsonPropertyName("payable_with")]//Opções de pagamento
    public string[] PayableWith { get; set; } = payableWith;
    [JsonPropertyName("ensure_workday_due_date")]//Data de vencimento apenas em dias de semana
    public bool EnsureWorkDayDueDate { get; set; } = false;
}

public sealed class IuguFaturaItem(string description, int quantity, int priceCents)
{
    [JsonPropertyName("description")]//Descrição do item
    public string Description { get; set; } = description;
    [JsonPropertyName("quantity")]//Quantidade do item
    public int Quantity { get; set; } = quantity;
    [JsonPropertyName("price_cents")]//Preço do item em centavos. Valor mínimo 100.
    public int PriceCents { get; set; } = priceCents;
}

public sealed class IuguFaturaSplit(string recipientAccountId, float percent)
{
    [JsonPropertyName("recipient_account_id")]//Id da Subconta
    public string RecipientAccountId { get; set; } = recipientAccountId;
    [JsonPropertyName("percent")]//Porcentagem a ser cobrada da fatura
    public float Percent { get; set; } = percent;
}