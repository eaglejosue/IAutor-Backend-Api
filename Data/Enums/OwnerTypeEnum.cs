namespace IAutor.Api.Data.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OwnerTypeEnum
{
    [EnumMember(Value = "Influencer")] Influencer,
    [EnumMember(Value = "Agent")] Agent,
    [EnumMember(Value = "Other")] Other
}
