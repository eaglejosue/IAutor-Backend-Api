
namespace IAutor.Api.Endpoints;


public static class QuestionEndpoint
{
    public static void MapQuestionEndpoints(this WebApplication app)
    {
        const string ModelName = nameof(Question);
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };

        app.MapPost("/api/questions",
       async (
           Question model,
           [FromServices] IQuestionService service,
           [FromServices] INotificationService notification) =>
       {
           var entitie = await service.CreateAsync(model);
           if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
           return Results.Created($"/questions/{entitie!.Id}", entitie);
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
    }
}
