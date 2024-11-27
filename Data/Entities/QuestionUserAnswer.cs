namespace IAutor.Api.Data.Entities;

public class QuestionUserAnswer
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public long ChapterId { get; set; }
    public long QuestionId { get; set; }
    public long UserId { get; set; }
    public long BookId { get; set; }
    public string Answer { get; set; }
    public long QtdCallIASugestionsUsed { get; set; }
    public string? ImagePhotoUrl { get; set; }
    public string? ImagePhotoThumbUrl { get; set; }
    public string? ImagePhotoLabel { get; set; }
    public DateTime? ImagePhotoUploadDate { get; set; }

    public string? ImagePhotoOriginalFileName { get; set; }

    [JsonIgnore] public Chapter? Chapter { get; set; }
    [JsonIgnore] public Question? Question { get; set; }
    [JsonIgnore] public User? User { get; set; }
    [JsonIgnore] public Book? Book { get; set; }
}
