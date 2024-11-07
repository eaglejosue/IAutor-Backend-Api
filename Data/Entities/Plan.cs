namespace IAutor.Api.Data.Entities;

public class Plan : Base
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "R$";
    public long MaxLimitSendDataIA { get; set; }
    public DateTime InitialValidityPeriod { get; set; } = DateTimeBr.Now;
    public DateTime? FinalValidityPeriod { get; set; }
    public decimal CaractersLimitFactor { get; set; }

    public ICollection<PlanChapter> PlanChapters { get; set; } = [];

    [NotMapped] public IList<AddNewPlanQuestionRequest> ChapterPlanQuestion { get; set; }
}

public class AddNewPlanQuestionRequest
{
    public long ChapterId { get; set; }
    public long QuestionId { get; set; }
}