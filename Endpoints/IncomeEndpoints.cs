namespace IAutor.Api.Endpoints;

public static class IncomeEndpoints
{
    public static void MapIncomeEndpoints(this WebApplication app)
    {
        const string ModelName = nameof(Income);
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };

        app.MapGet("/api/incomes/{id:long}",
        async (long id, [FromServices] IIncomeService service) =>
        {
            var entitie = await service.GetByIdAsync(id);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(Income))
        .WithName($"{ModelName}ById")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Returns one {ModelName}",
            Description = $"This endpoint receives an Id from the header and searches for it in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("GetIncomes");

        app.MapGet("/api/incomes",
        async ([AsParameters] IncomeFilters filters, [FromServices] IIncomeService service) =>
        {
            var entities = await service.GetAllAsync(filters);
            if (entities.Count == 0) return Results.NoContent();
            return Results.Ok(entities);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<Income>))
        .WithName($"All{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Get all {ModelName}s",
            Description = $"This endpoint searches for all records in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("GetIncomes");

        app.MapGet("/api/incomes/grouped-by-day",
        async ([AsParameters] IncomeGroupedFilters filters, [FromServices] IIncomeService service) =>
        {
            var entities = await service.GetGroupedByDayAsync(filters);
            if (entities.Count == 0) return Results.NoContent();
            return Results.Ok(entities);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<Income>))
        .WithName("GroupedByDay")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Get all {ModelName}s grouped by day",
            Description = $"This endpoint searches for all records in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("GetIncomes");
    }
}