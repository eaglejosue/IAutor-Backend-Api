namespace IAutor.Api.Data.Dtos.Iugu;

public sealed class IuguFinancialResponse : IuguResponse
{
    [JsonPropertyName("transactions")]
    public List<Transaction> Transactions { get; set; } = [];

    [JsonPropertyName("initial_balance")]
    public Balance InitialBalance { get; set; }

    [JsonPropertyName("initial_date")]
    public DateTime InitialDate { get; set; }

    [JsonPropertyName("final_date")]
    public DateTime FinalDate { get; set; }

    [JsonPropertyName("transactions_total")]
    public int TransactionsTotal { get; set; }
}

public class Balance
{
    [JsonPropertyName("amount")]
    public string Amount { get; set; }

    [JsonPropertyName("amount_cents")]
    public string AmountCents { get; set; }

    [JsonPropertyName("entry_date")]
    public DateTime EntryDate { get; set; }
}

public class Transaction
{
    //
}