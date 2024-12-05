namespace IAutor.Api.Endpoints;

public static class IAEndpoints
{
    public static void MapIAEndpoints(this WebApplication app)
    {
        var tag = new List<OpenApiTag> { new() { Name = "IA" } };

        app.MapPost("/api/ia",
        async (
            AiTextRequest model,
            [FromServices] IIAService service,
            [FromServices] INotificationService notification,
            HttpContext context) =>
        {
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var loggedUserName = TokenExtension.GetUserNameFromToken(context);
            var response = await service.GenerateIAResponse(model, loggedUserId, loggedUserName);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Ok(response);
        })
        .Produces((int)HttpStatusCode.OK)
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Create a new response from AI",
            Description = "This endpoint receives a question answer object as the request body and generate new response from IA. It produces a 200 status code.",
            Tags = tag
        });
    }
}
