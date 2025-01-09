namespace IAutor.Api.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        const string ModelName = nameof(Book);
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };

        app.MapGet("/api/books/{id:long}",
        async (long id, [FromServices] IBookService service) =>
        {
            var entitie = await service.GetByIdAsync(id);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(Book))
        .WithName($"{ModelName}ById")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Returns one {ModelName}",
            Description = $"This endpoint receives an Id from the header and searches for it in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapGet("/api/books",
        async (
            [AsParameters] BookFilters filters,
            [FromServices] IBookService service,
            HttpContext context) =>
        {
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var entities = await service.GetAllAsync(filters, loggedUserId);
            if (entities.Count == 0) return Results.NoContent();
            return Results.Ok(entities);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<Book>))
        .WithName($"All{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Get all {ModelName}s",
            Description = $"This endpoint searches for all records in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapPost("/api/books",
        async (
            Book model,
            [FromServices] IBookService service,
            [FromServices] INotificationService notification) =>
        {
            var entitie = await service.CreateAsync(model);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Created($"/Books/{entitie!.Id}", entitie);
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

        app.MapPut("/api/books",
        async (
            Book model,
            [FromServices] IBookService service,
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

        app.MapPatch("/api/books",
        async (
            Book model,
            [FromServices] IBookService service,
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

        app.MapDelete("/api/books/{id:long}",
        async (
            long id,
            [FromServices] IBookService service,
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

        app.MapGet("/api/books/{id:long}/pdf-v1",
        async (long id, [FromServices] IBookService service) =>
        {
            var pdfFile = await service.GetBookPDFv1(id);
            if (pdfFile is null) return Results.NoContent();
            return Results.Ok(pdfFile);
        })
        .Produces((int)HttpStatusCode.OK)
        .WithName("Book PDF v1")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Return a {ModelName} PDF",
            Description = $"This endpoint receives an Id from the body and returns the {ModelName} PDF. It produces a 200 status code.",
            Tags = tag
        });

        app.MapGet("/api/books/{id:long}/pdf",
        async (long id, [FromServices] IBookService service) =>
        {
            var pdfFile = await service.GetBookPDFv2(id);
            if (pdfFile is null) return Results.NoContent();
            return Results.Ok(pdfFile);
        })
        .Produces((int)HttpStatusCode.OK)
        .WithName("Book PDF v2")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Return a {ModelName} PDF",
            Description = $"This endpoint receives an Id from the body and returns the {ModelName} PDF. It produces a 200 status code.",
            Tags = tag
        });
    }
}