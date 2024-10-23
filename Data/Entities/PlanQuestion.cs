namespace IAutor.Api.Data.Entities;

public class PlanQuestion:Base
{
    public long PlanId { get; set; }
    public long QuestionId { get; set; }

    public Question Question { get; set; } = null!;

    public Plan Plan { get; set; } = null!;

}
