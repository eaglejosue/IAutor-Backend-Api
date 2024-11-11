namespace IAutor.Api.Data.Entities;

public sealed class Question : Base
{
    public string Title { get; set; }
    public int MaxLimitCharacters { get; set; }
    public int MinLimitCharacters { get; set; }
    public string Subject { get; set; }

    [JsonIgnore] public ICollection<PlanChapterQuestion>? PlansChaptersQuestions { get; set; }
    [JsonIgnore] public ICollection<QuestionUserAnswer>? QuestionUserAnswers { get; set; }

    [NotMapped] public bool Selected { get; set; }
    [NotMapped] public QuestionUserAnswer? QuestionUserAnswer => QuestionUserAnswers?.FirstOrDefault() ?? new();
}
