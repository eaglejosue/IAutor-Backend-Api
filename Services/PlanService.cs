namespace IAutor.Api.Services;

public interface IPlanService
{
    Task<Plan?> GetByIdAsync(long id);
    Task<List<Plan>> GetAllAsync(PlanFilters filters);
    Task<Plan?> CreateAsync(Plan model);
    Task<Plan?> UpdateAsync(Plan model, long loggedUserId, string loggedUserName);
    Task<Plan?> PatchAsync(Plan model, long loggedUserId, string loggedUserName);
    Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName);

    Task<List<PlanChapter>?> GetPlanChapterByIdPlanAsync(long planId);
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
        AddChapterQuestions(model);

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


    public async Task<List<PlanChapter>?> GetPlanChapterByIdPlanAsync(long planId) =>
        await db.PlansChapters.Where(r => r.PlanId == planId)
            .Include(r => r.Chapter).Include(r => r.PlanChapterQuestions).ThenInclude(g => g.Question)
            .ToListAsync().ConfigureAwait(false);


    private static void AddChapterQuestions(Plan model)
    {
        if (model.ChapterPlanQuestion.Count == 0)
            return;

        model.PlanChapters = model.ChapterPlanQuestion
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
