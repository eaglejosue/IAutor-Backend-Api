using System.ComponentModel.DataAnnotations;

namespace IAutor.Api.Data.Dtos.Picpay
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Customer
    {
        public Customer(string name, string email, DocumentType documentType, string document) =>
        (Name,Email,DocumentType,Document) = (name,email,documentType,document);

        [JsonPropertyName("name")]
        [StringLength(255)]
        public required string Name { get; init; }

        [JsonPropertyName("email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public required string Email { get; init; }

        [JsonPropertyName("documentType")]
        public DocumentType DocumentType { get; init; }

        [JsonPropertyName("document")]
        public string Document { get; init; }

        [JsonPropertyName("phone")]
        public Phone Phone { get; set; }
    }

    public class Phone
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }

        [JsonPropertyName("areaCode")]
        public string AreaCode { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }
    }

    public class Receiver
    {
        [JsonPropertyName("merchantCode")]
        public string MerchantCode { get; set; }

        [JsonPropertyName("mdrDiscount")]
        public bool MdrDiscount { get; set; }

        [JsonPropertyName("numbchargeRemainderer")]
        public bool ChargeRemainder { get; set; }

        [JsonPropertyName("fixedAmount")]
        public Int32 FixedAmount { get; set; }

        [JsonPropertyName("percentageAmount")]
        public double PercentageAmount { get; set; }

        [JsonPropertyName("commissionFixedAmount")]
        public Int32 CommissionFixedAmount { get; set; }

        [JsonPropertyName("commissionPercentageAmount")]
        public double CommissionPercentageAmount { get; set; }
    }

    public class PicPayCheckOut
    {
        [JsonPropertyName("amount")]
        public required Int32 Amount { get; set; }

        [JsonPropertyName("customer")]
        public required Customer Customer { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("maxInstallmentNumber")]
        public Int32 MaxInstallmentNumber { get; set; }

        [JsonPropertyName("planId")]
        public string PlanId { get; set; }

        [JsonPropertyName("shippingAmount")]
        public Int32 ShippingAmount { get; set; }

        [JsonPropertyName("shippingAddress")]
        public ShippingAddress ShippingAddress { get; set; }

        [JsonPropertyName("receivers")]
        public List<Receiver> Receivers { get; set; }

        [JsonPropertyName("threeDomainSecurePolicy")]
        public string ThreeDomainSecurePolicy { get; set; } = "INACTIVE";
    }

    public class ShippingAddress
    {
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonPropertyName("complement")]
        public string Complement { get; set; }


    }

    public enum DocumentType
    {
        CPF,
        CNPJ,
        PASSPORT
    }


}
