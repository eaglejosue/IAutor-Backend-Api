namespace IAutor.Api.Data.Dtos.ViewModel;

public class AddNewPlanRequest
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public long MaxLimitSendDataIA { get; set; }
    public DateTime InitialValidityPeriod { get; set; }
    public DateTime? FinalValidityPeriod { get; set; }
    public decimal CaractersLimitFactor { get; set; }
    public IList<AddNewPlanQuestionRequest> ChapterPlanQuestion { get; set; }
}

public class AddNewPlanQuestionRequest
{
    public long ChapterID { get; set; }

    public long QuestionId { get; set; }
}
