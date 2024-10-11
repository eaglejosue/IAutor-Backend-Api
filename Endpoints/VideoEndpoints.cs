namespace IAutor.Api.Endpoints;

public static class VideoEndpoints
{
    public static void MapVideoEndpoints(this WebApplication app)
    {
        const string ModelName = nameof(Video);
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };

        app.MapGet("/api/videos/{id:long}",
        async (long id, [FromServices] IVideoService service) =>
        {
            var entitie = await service.GetByIdAsync(id);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(Video))
        .WithName($"{ModelName}ById")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Returns one {ModelName}",
            Description = $"This endpoint receives an Id from the header and searches for it in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapGet("/api/videos",
        async (
            [AsParameters] VideoFilters filters,
            [FromServices] IVideoService service,
            HttpContext context) =>
        {
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var entities = await service.GetAllAsync(filters, loggedUserId);
            if (entities.Count == 0) return Results.NoContent();
            return Results.Ok(entities);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<Video>))
        .WithName($"All{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Get all {ModelName}s",
            Description = $"This endpoint searches for all records in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapPost("/api/videos",
        async (
            Video model,
            [FromServices] IVideoService service,
            [FromServices] INotificationService notification) =>
        {
            var entitie = await service.CreateAsync(model);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Created($"/videos/{entitie!.Id}", entitie);
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

        app.MapPut("/api/videos",
        async (
            Video model,
            [FromServices] IVideoService service,
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

        app.MapPatch("/api/videos",
        async (
            Video model,
            [FromServices] IVideoService service,
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

        app.MapDelete("/api/videos/{id:long}",
        async (
            long id,
            [FromServices] IVideoService service,
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

        app.MapGet("/api/videos/trailer/{id:long}",
        async (long id, [FromServices] IVideoService service) =>
        {
            var entitie = await service.GetVideoTrailerByIdAsync(id);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(Video))
        .WithName("VideoTrailerById")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Get all VideoTrailers",
            Description = $"This endpoint receives an Id from the header and searches for it in the VideoTrailers table by Id. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("Get");

        app.MapGet("/api/videos/{id:long}/trailers",
        async (long id, [FromServices] IVideoService service) =>
        {
            var entities = await service.GetVideoTrailersByVideoIdAsync(id);
            if (entities.Count == 0) return Results.NoContent();
            return Results.Ok(entities);
        })
        .Produces((int)HttpStatusCode.OK, typeof(Video))
        .WithName("VideoTrailersByVideoId")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Get all VideoTrailers",
            Description = $"This endpoint receives an Id from the header and searches for it in the VideoTrailers table by VideoId. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("Get");

        app.MapPost("/api/videos/trailers",
        async (
            VideoTrailer model,
            [FromServices] IVideoService service,
            [FromServices] INotificationService notification) =>
        {
            var entitie = await service.AddVideoTrailerAsync(model);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Created($"/videos/trailers/{entitie!.Id}", entitie);
        })
        .Produces((int)HttpStatusCode.Created)
        .WithName("AddVideoTrailer")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Add VideoTrailer",
            Description = "This endpoint receives a VideoTrailer object as the request body and add it in the VideoTrailers table. It produces a 201 status code.",
            Tags = tag
        })
        .RequireAuthorization("Create");

        app.MapDelete("/api/videos/trailers/{id:long}",
        async (
            long id,
            [FromServices] IVideoService service,
            [FromServices] INotificationService notification) =>
        {
            var result = await service.RemoveVideoTrailerAsync(id);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            if (result == null) return Results.NoContent();
            return Results.Ok(result);
        })
        .Produces((int)HttpStatusCode.OK)
        .WithName("RemoveVideoTrailer")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Remove VideoTrailer",
            Description = "This endpoint receives an Id from the header and deletes it from the VideoTrailers table. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("Delete");
    }
}