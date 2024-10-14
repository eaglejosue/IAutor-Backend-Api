namespace IAutor.Api.Data.Entities;

public sealed class BookDegust
{
    public long Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string Title { get; set; }
    public long BookId { get; set; }
    public string PublicId { get; set; }

    [JsonIgnore] public Book? Book { get; set; }

    internal void Inactivate()
    {
        IsActive = false;
        DeletedAt = DateTimeBr.Now;
    }
}