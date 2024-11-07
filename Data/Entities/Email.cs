namespace IAutor.Api.Data.Entities;

public sealed class Email : Base
{
    public long UserId { get; set; }
    public long? BookId { get; set; }
    public EmailType? EmailType { get; set; }
    public DateTime? ScheduleDate { get; set; }
    public DateTime? DateSent { get; set; }
    public int? SendAttempts { get; set; }

    [JsonIgnore] public User? User { get; set; }
    [JsonIgnore] public Book? Book { get; set; }

    public Email() { }

    public Email(
        long userId,
        EmailType emailType,
        DateTime? scheduleDate = null,
        long? BookId = null)
    {
        UserId = userId;
        EmailType = emailType;
        ScheduleDate = scheduleDate;
        BookId = BookId;
    }
}