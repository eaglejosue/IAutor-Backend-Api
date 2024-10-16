namespace IAutor.Api.Data.Entities
{
    public sealed class Theme:Base
    {
        public string Title { get; set; }

        [JsonIgnore] public ICollection<Question>? Questions { get; set; }
    }
}
