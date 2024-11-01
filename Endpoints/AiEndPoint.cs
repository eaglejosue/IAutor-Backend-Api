namespace IAutor.Api.Endpoints;

public static class AiEndpoints
{
    public static void MapAiEndpoints(this WebApplication app)
    {
        const string ModelName = "AI";
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };

        app.MapPost("/api/AI",
        async (
            AiTextRequest model,
            [FromServices] IAiService service,
            [FromServices] INotificationService notification) =>
        {
            var response = await service.GenerateIaResponse(model);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Ok(response);
        })
        .Produces((int)HttpStatusCode.OK)
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Create a new response from AI",
            Description = $"This endpoint receives a {ModelName} object as the request body and generate new response from AI. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("Create");

      
    }
}
