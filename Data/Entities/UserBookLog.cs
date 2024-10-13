namespace IAutor.Api.Data.Entities;

public sealed class UserBookLog
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
    public long BookId { get; set; }
    public string Log { get; set; }

    [JsonIgnore] public User? User { get; set; }
    [JsonIgnore] public Book? Book { get; set; }
}