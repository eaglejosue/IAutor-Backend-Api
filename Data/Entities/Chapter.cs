namespace IAutor.Api.Data.Entities
{
    public class Chapter:Base
    {
        public string? ChapterNumber { get; set; }
        public string Title { get; set; }

        [JsonIgnore] public ICollection<Question>? Questions { get; set; }

        #region Methods

        public Chapter() { }

        #endregion
    }
}
