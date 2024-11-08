namespace IAutor.Api.Endpoints;

public static class IAEndpoints
{
    public static void MapIAEndpoints(this WebApplication app)
    {
        const string ModelName = "IA";
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };

        app.MapPost("/api/ia",
        async (
            AiTextRequest model,
            [FromServices] IAiService service,
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
            Summary = $"Create a new response from AI",
            Description = $"This endpoint receives a {ModelName} object as the request body and generate new response from IA. It produces a 200 status code.",
            Tags = tag
        });
    }
}
