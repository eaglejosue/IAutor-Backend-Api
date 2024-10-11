namespace IAutor.Api.Data.Dtos.Iugu;

public sealed class IuguAccountInfoResponse : IuguResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string AccountName { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("can_receive?")]
    public bool CanReceive { get; set; }

    [JsonPropertyName("is_verified?")]
    public bool IsVerified { get; set; }

    [JsonPropertyName("last_verification_request_status")]
    public string LastVerificationRequestStatus { get; set; }

    [JsonPropertyName("last_verification_request_data")]
    public object LastVerificationRequestData { get; set; }

    [JsonPropertyName("last_verification_request_feedback")]
    public object LastVerificationRequestFeedback { get; set; }

    [JsonPropertyName("change_plan_type")]
    public int ChangePlanType { get; set; }

    [JsonPropertyName("subscriptions_trial_period")]
    public int SubscriptionsTrialPeriod { get; set; }

    [JsonPropertyName("subscriptions_billing_days")]
    public int SubscriptionsBillingDays { get; set; }

    [JsonPropertyName("disable_emails")]
    public bool DisableEmails { get; set; }

    [JsonPropertyName("last_withdraw")]
    public object LastWithdraw { get; set; }

    [JsonPropertyName("reply_to")]
    public string ReplyTo { get; set; }

    [JsonPropertyName("webapp_on_test_mode")]
    public bool WebappOnTestMode { get; set; }

    [JsonPropertyName("marketplace")]
    public bool Marketplace { get; set; }

    [JsonPropertyName("default_return_url")]
    public object DefaultReturnUrl { get; set; }

    [JsonPropertyName("credit_card_verified")]
    public bool CreditCardVerified { get; set; }

    [JsonPropertyName("fines")]
    public object Fines { get; set; }

    [JsonPropertyName("late_payment_fine")]
    public object LatePaymentFine { get; set; }

    [JsonPropertyName("per_day_interest")]
    public object PerDayInterest { get; set; }

    [JsonPropertyName("old_advancement")]
    public object OldAdvancement { get; set; }

    [JsonPropertyName("early_payment_discount")]
    public object EarlyPaymentDiscount { get; set; }

    [JsonPropertyName("early_payment_discount_days")]
    public object EarlyPaymentDiscountDays { get; set; }

    [JsonPropertyName("early_payment_discount_percent")]
    public object EarlyPaymentDiscountPercent { get; set; }

    [JsonPropertyName("sac_phone")]
    public object SacPhone { get; set; }

    [JsonPropertyName("sac_email")]
    public object SacEmail { get; set; }

    [JsonPropertyName("informations")]
    public List<Information> Informations { get; set; }

    [JsonPropertyName("configuration")]
    public Configuration Configuration { get; set; }

    [JsonPropertyName("bank_accounts")]
    public List<BankAccount> BankAccounts { get; set; }

    [JsonPropertyName("auto_withdraw")]
    public bool AutoWithdraw { get; set; }

    [JsonPropertyName("disabled_withdraw")]
    public bool DisabledWithdraw { get; set; }

    [JsonPropertyName("payment_email_notification")]
    public bool PaymentEmailNotification { get; set; }

    [JsonPropertyName("payment_email_notification_receiver")]
    public object PaymentEmailNotificationReceiver { get; set; }

    [JsonPropertyName("auto_advance")]
    public bool AutoAdvance { get; set; }

    [JsonPropertyName("auto_advance_type")]
    public object AutoAdvanceType { get; set; }

    [JsonPropertyName("auto_advance_option")]
    public object AutoAdvanceOption { get; set; }

    [JsonPropertyName("balance")]
    public string Balance { get; set; }

    [JsonPropertyName("balance_in_protest")]
    public string BalanceInProtest { get; set; }

    [JsonPropertyName("balance_available_for_withdraw")]
    public string BalanceAvailableForWithdraw { get; set; }

    [JsonPropertyName("protected_balance")]
    public string ProtectedBalance { get; set; }

    [JsonPropertyName("payable_balance")]
    public string PayableBalance { get; set; }

    [JsonPropertyName("receivable_balance")]
    public string ReceivableBalance { get; set; }

    [JsonPropertyName("commission_balance")]
    public string CommissionBalance { get; set; }

    [JsonPropertyName("volume_last_month")]
    public string VolumeLastMonth { get; set; }

    [JsonPropertyName("volume_this_month")]
    public string VolumeThisMonth { get; set; }

    [JsonPropertyName("total_subscriptions")]
    public int TotalSubscriptions { get; set; }

    [JsonPropertyName("total_active_subscriptions")]
    public int TotalActiveSubscriptions { get; set; }

    [JsonPropertyName("taxes_paid_last_month")]
    public string TaxesPaidLastMonth { get; set; }

    [JsonPropertyName("taxes_paid_this_month")]
    public string TaxesPaidThisMonth { get; set; }

    [JsonPropertyName("has_bank_address?")]
    public bool HasBankAddress { get; set; }

    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; }

    [JsonPropertyName("custom_logo_url")]
    public object CustomLogoUrl { get; set; }

    [JsonPropertyName("custom_logo_small_url")]
    public object CustomLogoSmallUrl { get; set; }

    [JsonPropertyName("early_payment_discounts")]
    public List<object> EarlyPaymentDiscounts { get; set; }

    [JsonPropertyName("commissions")]
    public object Commissions { get; set; }

    [JsonPropertyName("splits")]
    public List<object> Splits { get; set; }

    [JsonPropertyName("contact_data")]
    public ContactData ContactData { get; set; }
}

public class Information
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public class Configuration
{
    [JsonPropertyName("auto_withdraw")]
    public bool AutoWithdraw { get; set; }

    [JsonPropertyName("disabled_withdraw")]
    public bool DisabledWithdraw { get; set; }

    [JsonPropertyName("payment_email_notification")]
    public bool PaymentEmailNotification { get; set; }

    [JsonPropertyName("payment_email_notification_receiver")]
    public object PaymentEmailNotificationReceiver { get; set; }

    [JsonPropertyName("auto_advance")]
    public bool? AutoAdvance { get; set; }

    [JsonPropertyName("auto_advance_type")]
    public object AutoAdvanceType { get; set; }

    [JsonPropertyName("auto_advance_option")]
    public object AutoAdvanceOption { get; set; }

    [JsonPropertyName("commission_percent")]
    public int CommissionPercent { get; set; }

    [JsonPropertyName("owner_emails_to_notify")]
    public object OwnerEmailsToNotify { get; set; }

    [JsonPropertyName("fines")]
    public object Fines { get; set; }

    [JsonPropertyName("late_payment_fine")]
    public object LatePaymentFine { get; set; }

    [JsonPropertyName("per_day_interest")]
    public object PerDayInterest { get; set; }

    [JsonPropertyName("bank_slip")]
    public BankSlip BankSlip { get; set; }

    [JsonPropertyName("credit_card")]
    public CreditCard CreditCard { get; set; }

    [JsonPropertyName("pix")]
    public Pix Pix { get; set; }

    [JsonPropertyName("early_payment_discount")]
    public object EarlyPaymentDiscount { get; set; }

    [JsonPropertyName("early_payment_discount_days")]
    public object EarlyPaymentDiscountDays { get; set; }

    [JsonPropertyName("early_payment_discount_percent")]
    public object EarlyPaymentDiscountPercent { get; set; }
}

public class BankSlip
{
    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("extra_due")]
    public string ExtraDue { get; set; }

    [JsonPropertyName("reprint_extra_due")]
    public string ReprintExtraDue { get; set; }
}

public class CreditCard
{
    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("soft_descriptor")]
    public string SoftDescriptor { get; set; }

    [JsonPropertyName("installments")]
    public bool Installments { get; set; }

    [JsonPropertyName("installments_pass_interest")]
    public bool InstallmentsPassInterest { get; set; }

    [JsonPropertyName("max_installments")]
    public string MaxInstallments { get; set; }

    [JsonPropertyName("max_installments_without_interest")]
    public string MaxInstallmentsWithoutInterest { get; set; }

    [JsonPropertyName("two_step_transaction")]
    public bool TwoStepTransaction { get; set; }
}

public class Pix
{
    [JsonPropertyName("active")]
    public bool Active { get; set; }
}

public class BankAccount
{
    [JsonPropertyName("branch")]
    public string Branch { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("digit")]
    public string Digit { get; set; }
}

public class ContactData
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("document_number")]
    public string DocumentNumber { get; set; }

    [JsonPropertyName("full_address")]
    public string FullAddress { get; set; }
}