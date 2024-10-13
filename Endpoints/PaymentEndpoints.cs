namespace IAutor.Api.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this WebApplication app)
    {
        const string ModelName = nameof(Payment);
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };

        app.MapGet("/api/payments/{id:long}",
        async (long id, [FromServices] IPaymentService service) =>
        {
            var entitie = await service.GetByIdAsync(id);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(Payment))
        .WithName($"{ModelName}ById")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Returns one {ModelName}",
            Description = $"This endpoint receives an Id from the header and searches for it in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("Get");

        app.MapGet("/api/payments",
        async ([AsParameters] PaymentFilters filters, [FromServices] IPaymentService service) =>
        {
            var entities = await service.GetAllAsync(filters);
            if (entities.Count == 0) return Results.NoContent();
            return Results.Ok(entities);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<Payment>))
        .WithName($"All{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Get all {ModelName}s",
            Description = $"This endpoint searches for all records in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("Get");

        app.MapPost("/api/payments",
        async (
            Payment model,
            [FromServices] IPaymentService service,
            [FromServices] INotificationService notification) =>
        {
            var entitie = await service.CreateAsync(model);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Created($"/payments/{entitie!.Id}", entitie);
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

        app.MapPut("/api/payments",
        async (
            Payment model,
            [FromServices] IPaymentService service,
            [FromServices] INotificationService notification) =>
        {
            var entitie = await service.UpdateAsync(model);
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

        app.MapPatch("/api/payments",
        async (
            Payment model,
            [FromServices] IPaymentService service,
            [FromServices] INotificationService notification) =>
        {
            var entitie = await service.PatchAsync(model);
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

        app.MapDelete("/api/payments/{id:long}",
        async (
            long id,
            [FromServices] IPaymentService service,
            [FromServices] INotificationService notification) =>
        {
            var result = await service.DeleteAsync(id);
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

        app.MapPost("/api/payments/status",
        async (
            IuguPaymentChangedStatus model,
            [FromServices] IPaymentService service) =>
        {
            var entitie = await service.UpdateStatusAsync(model);
            if (entitie is null) return Results.NotFound();
            return Results.Ok();
        })
        .Produces((int)HttpStatusCode.OK)
        .WithName($"UpdateStatus{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Updates Status one {ModelName}",
            Description = $"This endpoint receives a Changed Status from Iugu Trigger in the body and updates {ModelName}s status table. It produces a 200 status code.",
            Tags = tag
        }).DisableAntiforgery();
    }
}