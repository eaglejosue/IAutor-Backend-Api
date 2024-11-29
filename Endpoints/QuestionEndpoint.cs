namespace IAutor.Api.Endpoints;

public static class QuestionEndpoint
{
    public static void MapQuestionEndpoints(this WebApplication app)
    {
        const string ModelName = nameof(Question);
        var tag = new List<OpenApiTag> { new() { Name = ModelName } };

        app.MapGet("/api/questions/{id:long}",
        async (long id, [FromServices] IQuestionService service) =>
        {
            var entitie = await service.GetByIdAsync(id);
            if (entitie is null) return Results.NoContent();
            return Results.Ok(entitie);
        })
        .Produces((int)HttpStatusCode.OK, typeof(Question))
        .WithName($"{ModelName}ById")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Returns one {ModelName}",
            Description = $"This endpoint receives an Id from the header and searches for it in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapGet("/api/questions",
        async (
            [AsParameters] QuestionFilters filters,
            [FromServices] IQuestionService service,
            HttpContext context) =>
        {
            var entities = await service.GetAllAsync(filters);
            if (entities.Count == 0) return Results.NoContent();
            return Results.Ok(entities);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<Question>))
        .WithName($"All{ModelName}")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = $"Get all {ModelName}s",
            Description = $"This endpoint searches for all records in the {ModelName}s table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapPost("/api/questions",
        async (
            Question model,
            [FromServices] IQuestionService service,
            [FromServices] INotificationService notification) =>
        {
            var entitie = await service.CreateAsync(model);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Created($"/Questions/{entitie!.Id}", entitie);
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

        app.MapPut("/api/questions",
        async (
            Question model,
            [FromServices] IQuestionService service,
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

        app.MapPatch("/api/questions",
        async (
            Question model,
            [FromServices] IQuestionService service,
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

        app.MapDelete("/api/questions/{id:long}",
        async (
            long id,
            [FromServices] IQuestionService service,
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

        //QuestionUserAnswer

        app.MapGet("/api/questions/user-answers-by-book/{bookId:long}",
        async (
            long bookId,
            [FromServices] IQuestionService service,
            [FromServices] INotificationService notification,
            HttpContext context) =>
        {
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var entities = await service.GetQuestionUserAnswersAsync(loggedUserId, bookId);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            if (entities.Count == 0) return Results.NoContent();
            return Results.Ok(entities);
        })
        .Produces((int)HttpStatusCode.OK, typeof(List<Question>))
        .WithName($"AllQuestionUserAnswersByBookId")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get all QuestionUserAnswers",
            Description = "This endpoint searches for all records in the QuestionUserAnswers table. It produces a 200 status code.",
            Tags = tag
        });

        app.MapPost("/api/questions/user-answers",
        async (
            QuestionUserAnswer model,
            [FromServices] IQuestionService service,
            [FromServices] INotificationService notification,
            HttpContext context) =>
        {
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var loggedUserName = TokenExtension.GetUserNameFromToken(context);
            await service.UpsertQuestionUserAnswerAsync(model, loggedUserId, loggedUserName);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Ok();
        })
        .Produces((int)HttpStatusCode.OK)
        .WithName("CreateQuestionUserAnswer")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Create a new QuestionUserAnswer",
            Description = "This endpoint receives a QuestionUserAnswer object as the request body and add it in the QuestionUserAnswers table. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("Create");


        app.MapPost("/api/questions/uploadPhotoQuestionUserAnswer/{idQuestionUserAnwser:long}/{label}", 
        async (
            long idQuestionUserAnwser,
            string label, 
            IFormFile file,
            [FromServices] IQuestionService service,
            [FromServices] INotificationService notification,
            HttpContext context) =>
        {
            // File upload logic here
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var loggedUserName = TokenExtension.GetUserNameFromToken(context);
            await service.UploadPhotoQuestionUserAnswer(idQuestionUserAnwser,  file, label, loggedUserId, loggedUserName);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Created();


        }).Produces((int)HttpStatusCode.OK)
        .WithName("uploadPhotoQuestionUserAnswer")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Create a new Photo",
            Description = "This endpoint receives a Photo object as the request body and add it in the QuestionUserAnswers table. It produces a 200 status code.",
            Tags = tag
        }).DisableAntiforgery()
        .RequireAuthorization("Create");

        app.MapPut("/api/questions/user-answers-edit-photos",
        async (
            QuestionUserAnswer model,
            [FromServices] IQuestionService service,
            [FromServices] INotificationService notification,
            HttpContext context) =>
        {
            var loggedUserId = TokenExtension.GetUserIdFromToken(context);
            var loggedUserName = TokenExtension.GetUserNameFromToken(context);
            await service.UpdateQuestionUserPhotoAnswerAsync(model, loggedUserId, loggedUserName);
            if (notification.HasNotifications) return Results.BadRequest(notification.Notifications);
            return Results.Ok();
        })
        .Produces((int)HttpStatusCode.OK)
        .WithName("UpdateQuestionUserAnswerPhotos")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Update a Photo from QuestionUserAnswer",
            Description = "This endpoint receives a QuestionUserAnswer object as the request body and edit it in the QuestionUserAnswers table. It produces a 200 status code.",
            Tags = tag
        })
        .RequireAuthorization("Update");
    }
}
