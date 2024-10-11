namespace Pay4Tru.Api.Data.Model;

public sealed class Email : Base
{
    public long UserId { get; set; }
    public long? VideoId { get; set; }
    public EmailTypeEnum? EmailType { get; set; }
    public DateTime? ScheduleDate { get; set; }
    public DateTime? DateSent { get; set; }
    public int? SendAttempts { get; set; }

    [JsonIgnore] public User? User { get; set; }
    [JsonIgnore] public Video? Video { get; set; }

    public Email() { }

    public Email(
        long userId,
        EmailTypeEnum emailType,
        DateTime? scheduleDate = null,
        long? videoId = null)
    {
        UserId = userId;
        EmailType = emailType;
        ScheduleDate = scheduleDate;
        VideoId = videoId;
    }
}