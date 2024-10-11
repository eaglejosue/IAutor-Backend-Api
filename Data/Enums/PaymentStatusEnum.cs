namespace IAutor.Api.Data.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatusEnum
{
    [Description("Pendente")] Pending,
    [Description("Efetuado")] Paid,
    [Description("Cancelado")] Canceled,
    [Description("Efetuado Parcialmente")] PartiallyPaid,
    [Description("Devolvido")] Refunded,
    [Description("Expirado")] Expired,
    [Description("Autorizado")] Authorized,
    [Description("Efetuado Externamente")] ExternallyPaid,
    [Description("Em Protesto")] InProtest,
    [Description("Estornado")] Chargeback
}