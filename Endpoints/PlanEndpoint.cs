namespace IAutor.Api.Endpoints;

public static class PlanEndpoint
{
    public static void MapPlanEndpoints(this WebApplication app)
    {
        const string ModelName = nameof(Plan);
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };

        app.MapGet("/api/plans/{id:long}",
        async (long id, [FromServices] IPlanService service) =>
        {
            var entitie = await service.GetByIdAsync(id);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(Plan))
        .WithName($"{ModelName}ById")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Returns one {ModelName}",
            Description = $"This endpoint receives an Id from the header and searches for it in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapGet("/api/plans",
        async (
            [AsParameters] PlanFilters filters,
            [FromServices] IPlanService service,
            HttpContext context) =>
        {
            var entities = await service.GetAllAsync(filters);
            if (entities.Count == 0) return Results.NoContent();
            return Results.Ok(entities);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<Plan>))
        .WithName($"All{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Get all {ModelName}s",
            Description = $"This endpoint searches for all records in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapPost("/api/plans",
        async (
            Plan model,
            [FromServices] IPlanService service,
            [FromServices] INotificationService notification) =>
        {
            var entitie = await service.CreateAsync(model);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Created($"/Plans/{entitie!.Id}", entitie);
        })
        .Produces((int)HttpStatusCode.Created)
        .WithName($"Create{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Create a new {ModelName}",
            Description = $"This endpoint receives a {ModelName} object as the request body and add it in the {ModelName}s table. It produces a 201 status code.",
            Tags = tag
        })
        .RequireAuthorization("Create");

        app.MapPut("/api/plans",
        async (
            Plan model,
            [FromServices] IPlanService service,
            [FromServices] INotificationService notification,
            HttpContext context) =>
        {
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var loggedUserName = TokenExtension.GetUserNameFromToken(context);
            var entitie = await service.UpdateAsync(model, loggedUserId, loggedUserName);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK)
        .WithName($"Update{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Updates one {ModelName}",
            Description = $"This endpoint receives an Id through the header and a {ModelName} object as the request body and updates it in the {ModelName}s table. It produces a 201 status code.",
            Tags = tag
        })
        .RequireAuthorization("Update");

        app.MapPatch("/api/plans",
        async (
            Plan model,
            [FromServices] IPlanService service,
            [FromServices] INotificationService notification,
            HttpContext context) =>
        {
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var loggedUserName = TokenExtension.GetUserNameFromToken(context);
            var entitie = await service.PatchAsync(model, loggedUserId, loggedUserName);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK)
        .WithName($"Patch{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Activates/Deactivates an {ModelName}.",
            Description = $"This endpoint receives an Id through the header and a {ModelName} object as the request body and updates only changed properties it in the {ModelName}s table. It produces a 201 status code.",
            Tags = tag
        })
        .RequireAuthorization("Update");

        app.MapDelete("/api/plans/{id:long}",
        async (
            long id,
            [FromServices] IPlanService service,
            [FromServices] INotificationService notification,
            HttpContext context) =>
        {
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var loggedUserName = TokenExtension.GetUserNameFromToken(context);
            var result = await service.DeleteAsync(id, loggedUserId, loggedUserName);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            if (result == null) return Results.NoContent();
            return Results.Ok(result);
        })
        .Produces((int)HttpStatusCode.OK)
        .WithName($"Delete{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Delete one {ModelName}",
            Description = $"This endpoint receives an Id from the header and deletes it from the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("Delete");

        //Plan - Chapter - Questions

        app.MapGet("/api/plans/{planId:long}/planchapter",
        async (long planId, [FromServices] IPlanService service) =>
        {
            var entitie = await service.GetPlanChaptersByPlanIdAsync(planId);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<PlanChapter>))
        .WithName($"PlanChapterById")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Returns list of PlanChapter",
            Description = "This endpoint receives an Id from the header and searches for it in the PlanChapter table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapGet("/api/plans/{planId:long}/chapters-and-questions/book/{bookId:long}",
        async (long planId, long bookId, [FromServices] IPlanService service) =>
        {
            var entitie = await service.GetPlanChaptersAndQuestionsByPlanIdAsync(planId, bookId);
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<Plan>))
        .WithName($"PlanChaptersAndQuestionsByPlanId")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Returns Plan with Chapters and Questions",
            Description = "This endpoint receives an Id from the header and searches for it in the Plan table. It produces a 200 status code.",
            Tags = tag
        });
    }
}
