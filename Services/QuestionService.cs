namespace IAutor.Api.Services;

public interface IQuestionService
{
    Task<Question?> GetByIdAsync(long id);
    Task<List<Question>> GetAllAsync(QuestionFilters filters);
    Task<Question?> CreateAsync(Question model);
    Task<Question?> UpdateAsync(Question model, long loggedUserId, string loggedUserName);
    Task<Question?> PatchAsync(Question model, long loggedUserId, string loggedUserName);
    Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName);
    Task UpsertQuestionUserAnswerAsync(QuestionUserAnswer model, long loggedUserId, string loggedUserName);
    Task<List<QuestionUserAnswer>> GetQuestionUserAnswersAsync(long loggedUserId, long bookId);
}

public sealed class QuestionService(
    IAutorDb db,
    INotificationService notification,
    IUserService userService) : IQuestionService
{
    public async Task<Question?> GetByIdAsync(long id) => await db.Questions.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<List<Question>> GetAllAsync(QuestionFilters filters)
    {
        var predicate = PredicateBuilder.New<Question>(n => n.Id > 0);

        #region Filters

        if (filters.Id.HasValue)
            predicate.And(a => a.Id == filters.Id);

        if (filters.IsActive.HasValue)
            predicate.And(a => a.IsActive == filters.IsActive);

        if (filters.CreatedAt.HasValue && filters.CreatedAt > DateTime.MinValue)
            predicate.And(a => a.CreatedAt == filters.CreatedAt.Value.Date);

        if (filters.UpdatedAt.HasValue && filters.UpdatedAt > DateTime.MinValue)
            predicate.And(a => a.UpdatedAt == filters.UpdatedAt.Value.Date);

        if (filters.DeletedAt.HasValue && filters.DeletedAt > DateTime.MinValue)
            predicate.And(a => a.DeletedAt == filters.DeletedAt.Value.Date);

        if (!string.IsNullOrEmpty(filters.Title))
            predicate.And(a => EF.Functions.Like(a.Title, filters.Title.LikeConcat()));

        if (!string.IsNullOrEmpty(filters.Subject))
            predicate.And(a => EF.Functions.Like(a.Subject, filters.Subject.LikeConcat()));

        if ((filters?.ChapterId ?? 0) > 0)
            predicate.And(a => a.PlanChapterQuestions.Any(a => a.PlanChapter.ChapterId == filters.ChapterId));

        #endregion

        var query = db.Questions.Where(predicate);

        #region OrderBy

        query = filters?.OrderBy switch
        {
            "1" => query.OrderBy(o => o.Title),
            "2" => query.OrderBy(o => o.CreatedAt),
            "3" => query.OrderByDescending(o => o.CreatedAt),
            _ => query.OrderBy(o => o.Id)
        };

        #endregion

#if DEBUG
        var queryString = query.ToQueryString();
#endif

        return await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<Question?> CreateAsync(Question model)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var addResult = await db.Questions.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return addResult.Entity;
    }

    public async Task<Question?> UpdateAsync(Question model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidateUpdateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        return await UpdateEntityAsync(model, loggedUserId, loggedUserName, "Updated");
    }

    public async Task<Question?> PatchAsync(Question model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidatePatchAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        return await UpdateEntityAsync(model, loggedUserId, loggedUserName, "Patched");
    }

    private async Task<Question?> UpdateEntityAsync(Question model, long loggedUserId, string loggedUserName, string changeFrom)
    {
        var entitie = await db.Questions.FirstOrDefaultAsync(f => f.Id == model.Id).ConfigureAwait(false);

        if (entitie == null) return null;

        if (entitie.EntityUpdated(model))
        {
            entitie.UpdatedAt = DateTimeBr.Now;
            entitie.UpdatedBy = loggedUserName;
            db.Questions.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);

            await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Question Id {0} {1}", model.Id, changeFrom)));
        }

        return entitie;
    }

    public async Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName)
    {
        var entitie = await GetByIdAsync(id);
        if (entitie == null) return null;

        entitie.IsActive = false;
        entitie.DeletedAt = DateTimeBr.Now;
        entitie.UpdatedBy = loggedUserName;

        db.Questions.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);

        await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Question Id {0} Deleted", id)));

        return true;
    }

    public async Task UpsertQuestionUserAnswerAsync(QuestionUserAnswer model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return;
        }

        var entitie = await db.QuestionUserAnswers
            .FirstOrDefaultAsync(f =>
                f.ChapterId == model.ChapterId &&
                f.QuestionId == model.QuestionId &&
                f.UserId == model.UserId &&
                f.BookId == model.BookId
            ).ConfigureAwait(false);

        if (entitie == null)
        {
            model.CreatedAt = DateTimeBr.Now;
            await db.QuestionUserAnswers.AddAsync(model).ConfigureAwait(false);
        }
        else
        {
            entitie.UpdatedAt = DateTimeBr.Now;
            entitie.UpdatedBy = loggedUserName;
            entitie.Answer = model.Answer;
            entitie.QtdCallIASugestionsUsed = model.QtdCallIASugestionsUsed;
            db.QuestionUserAnswers.Update(model);
        }

        await db.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<List<QuestionUserAnswer>> GetQuestionUserAnswersAsync(long loggedUserId, long bookId) =>
        await db.QuestionUserAnswers.Where(w => w.UserId == loggedUserId && w.BookId == bookId).ToListAsync().ConfigureAwait(false);
}
