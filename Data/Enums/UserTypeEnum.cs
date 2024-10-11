namespace IAutor.Api.Data.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserTypeEnum
{
    [EnumMember(Value = "Default")] Default,
    [EnumMember(Value = "Admin")] Admin,
    [EnumMember(Value = "Operator")] Operator,
    [EnumMember(Value = "Influencer")] Influencer,
    [EnumMember(Value = "Agent")] Agent
}
