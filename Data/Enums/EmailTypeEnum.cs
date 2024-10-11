namespace IAutor.Api.Data.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmailTypeEnum
{
    [EnumMember(Value = "UserActivation")] UserActivation,
    [EnumMember(Value = "ForgotPassword")] ForgotPassword,
    [EnumMember(Value = "VideoReleaseSchedule")] VideoReleaseSchedule,
}
