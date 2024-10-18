
namespace IAutor.Api.Endpoints;


public static class ChapterEndpoint
{
    public static void MapChapterEndpoints(this WebApplication app)
    {
        const string ModelName = nameof(Chapter);
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };


        app.MapGet("/api/chapters",
      async ([AsParameters] ChapterFilters filters, [FromServices] IChapterService service) =>
      {
          
          var entities = await service.GetAllAsync(filters);
          if (entities.Count == 0) return Results.NoContent();
          return Results.Ok(entities);
      })
      .Produces((int)HttpStatusCode.OK, typeof(List<User>))
      .WithName($"All{ModelName}")
      .WithOpenApi(x => new OpenApiOperation(x)
      {
          Summary = $"Get all {ModelName}",
          Description = $"This endpoint searches for all records in the {ModelName}s table. It produces a 200 status code.",
          Tags = tag
      })
      .RequireAuthorization("Admin");

       
        app.MapPost("/api/chapters",
       async (
           Chapter model,
           [FromServices] IChapterService service,
           [FromServices] INotificationService notification) =>
       {
           var entitie = await service.CreateAsync(model);
           if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
           return Results.Created($"/chapters/{entitie!.Id}", entitie);
       })
       .Produces((int)HttpStatusCode.Created)
       .WithName($"Create{ModelName}")
       .WithOpenApi(x => new OpenApiOperation(x)
       {
           Summary = $"Create a new {ModelName}",
           Description = $"This endpoint receives a {ModelName} object as the request body and add it in the {ModelName}s table. It produces a 201 status code.",
           Tags = tag
       })
       .RequireAuthorization("Admin");

        app.MapPut("/api/chapters",
       async (
           Chapter model,
           [FromServices] IChapterService service,
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

        app.MapDelete("/api/chapters/{id:long}",
      async (
          long id,
          [FromServices] IChapterService service,
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
      .RequireAuthorization("Admin");
    }
}
