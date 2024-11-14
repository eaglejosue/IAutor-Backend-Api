namespace IAutor.Api.Services;

public interface IPlanService
{
    Task<Plan?> GetByIdAsync(long id);
    Task<List<Plan>> GetAllAsync(PlanFilters filters);
    Task<Plan?> CreateAsync(Plan model);
    Task<Plan?> UpdateAsync(Plan model, long loggedUserId, string loggedUserName);
    Task<Plan?> PatchAsync(Plan model, long loggedUserId, string loggedUserName);
    Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName);

    Task<List<PlanChapter>?> GetPlanChapterByPlanIdAsync(long planId);
    Task<Plan?> GetPlanChaptersAndQuestionsByPlanIdAsync(long planId, long bookId, long loggedUserId);
}

public sealed class PlanService(
    IAutorDb db,
    INotificationService notification,
    IUserService userService) : IPlanService
{
    public async Task<Plan?> GetByIdAsync(long id) => await db.Plans.Include(i=>i.PlanChapters).FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<List<Plan>> GetAllAsync(PlanFilters filters)
    {
        var predicate = PredicateBuilder.New<Plan>(n => n.Id > 0);

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

        #endregion

        var query = db.Plans.Where(predicate);

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

        return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<Plan?> CreateAsync(Plan model)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        AddChapterQuestions(model);

        model.CreatedAt = DateTimeBr.Now;

        if (string.IsNullOrEmpty(model.Currency))
            model.Currency = "R$";

        var addResult = await db.Plans.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return addResult.Entity;
    }

    public async Task<Plan?> UpdateAsync(Plan model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidateUpdateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        return await UpdateEntityAsync(model, loggedUserId, loggedUserName, "Updated");
    }

    public async Task<Plan?> PatchAsync(Plan model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidatePatchAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        return await UpdateEntityAsync(model, loggedUserId, loggedUserName, "Patched");
    }

    private async Task<Plan?> UpdateEntityAsync(Plan model, long loggedUserId, string loggedUserName, string changeFrom)
    {
        var entitie = await db.Plans.FirstOrDefaultAsync(f => f.Id == model.Id).ConfigureAwait(false);

        if (entitie == null) return null;

        if (model.PlanChapters.Count > 0)
        {
            foreach (var pc in model.PlanChapters)
                db.PlansChapters.Remove(pc);
        }

        AddChapterQuestions(model);

        if (entitie.EntityUpdated(model))
        {
            entitie.UpdatedAt = DateTimeBr.Now;
            entitie.UpdatedBy = loggedUserName;
            db.Plans.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);

            await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Plan Id {0} {1}", model.Id, changeFrom)));
        }

        return entitie;
    }

    public async Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName)
    {
        var entitie = await GetByIdAsync(id);
        if (entitie == null) return null;

        entitie.IsActive = false;
        entitie.DeletedAt = DateTime.UtcNow;
        entitie.UpdatedBy = loggedUserName;

        db.Plans.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);

        await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Plan Id {0} Deleted", id)));

        return true;
    }


    public async Task<List<PlanChapter>?> GetPlanChapterByPlanIdAsync(long planId)
    {
        var query = db.PlansChapters
            .Where(w => w.PlanId == planId)
            .Include(pc => pc.Chapter)
            .Include(pc => pc.PlanChapterQuestions)
                .ThenInclude(g => g.Question);

#if DEBUG
        var queryString = query.ToQueryString();
#endif

        return await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<Plan?> GetPlanChaptersAndQuestionsByPlanIdAsync(long planId, long bookId, long loggedUserId)
    {
        var query = db.PlansChapters
            .Where(w => w.PlanId == planId)
            .Include(pc => pc.Plan)
            .Include(pc => pc.Chapter)
            .Include(pc => pc.PlanChapterQuestions)
                .ThenInclude(g => g.Question)
                    .ThenInclude(q => q.QuestionUserAnswers);

        var result = await query
            .GroupBy(pc => pc.Plan) // Agrupa por plano
            .Select(g => new Plan
            {
                Id = g.Key.Id,
                Title = g.Key.Title,
                MaxQtdCallIASugestions = g.Key.MaxQtdCallIASugestions,
                Chapters = g.Select(pc => new Chapter
                {
                    Id = pc.Chapter.Id,
                    Title = pc.Chapter.Title,
                    ChapterNumber = pc.Chapter.ChapterNumber,
                    Questions = pc.PlanChapterQuestions.Select(pcq => new Question
                    {
                        Id = pcq.Question.Id,
                        Title = pcq.Question.Title,
                        MaxLimitCharacters = pcq.Question.MaxLimitCharacters,
                        MinLimitCharacters = pcq.Question.MinLimitCharacters,
                        QuestionUserAnswers = pcq.Question.QuestionUserAnswers
                            .Where(w =>
                                w.UserId == loggedUserId &&
                                w.BookId == bookId)
                            .ToList(),
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

#if DEBUG
        var queryString = query.ToQueryString();
#endif

        return result;
    }


    private static void AddChapterQuestions(Plan model)
    {
        if (model.ChapterQuestions.Count == 0)
            return;

        model.PlanChapters = model.ChapterQuestions
            .GroupBy(r => r.ChapterId)
            .Select(ch => new PlanChapter
            {
                ChapterId = ch.Key,
                PlanChapterQuestions = ch.Select(qu => new PlanChapterQuestion
                {
                    QuestionId = qu.QuestionId
                }).ToList()
            })
            .ToList();
    }
}
