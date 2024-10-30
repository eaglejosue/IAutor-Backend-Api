using IAutor.Api.Data.Dtos.ViewModel;

namespace IAutor.Api.Endpoints;

public static class PlanChapterEndpoint
{
    public static void MapPlanChapterEndpoints(this WebApplication app)
    {
        const string ModelName = nameof(PlanChapter);
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };


        app.MapGet("/api/planchapter/{idPlan:long}",
        async (long idPlan, [FromServices] IPlanService service) =>
        {
            var entitie = await service.GetPlanChapterByIdPlanAsync(idPlan);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<PlanChapter>))
        .WithName($"{ModelName}ById")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Returns list of {ModelName}",
            Description = $"This endpoint receives an Id from the header and searches for it in the {ModelName} table. It produces a 200 status code.",
            Tags = tag
        });

      
    }
}
