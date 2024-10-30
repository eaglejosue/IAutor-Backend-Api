using IAutor.Api.Data.Dtos.ViewModel;
using IAutor.Api.Data.Entities;

namespace IAutor.Api.Services;

public interface IPlanService
{
    Task<Plan?> GetByIdAsync(long id);

    Task<List<PlanChapter>?> GetPlanChapterByIdPlanAsync(long idPlan);
    Task<List<Plan>> GetAllAsync(PlanFilters filters);
    Task<Plan?> CreateAsync(AddNewPlanRequest model);
    Task<Plan?> UpdateAsync(AddNewPlanRequest model, long loggedUserId, string loggedUserName);
    Task<Plan?> PatchAsync(Plan model, long loggedUserId, string loggedUserName);
    Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName);
}
public sealed class PlanService(
    IAutorDb db,
    INotificationService notification,
    IUserService userService) : IPlanService
{
    public async Task<Plan?> GetByIdAsync(long id) => await db.Plans.Include(r=>r.PlanChapters).FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

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

    public async Task<Plan?> CreateAsync(AddNewPlanRequest model)
    {

        var plan = new Plan()
        {
            CaractersLimitFactor = model.CaractersLimitFactor,
            Currency = model.Currency,
            FinalValidityPeriod = model.FinalValidityPeriod,
            InitialValidityPeriod = model.InitialValidityPeriod,
            MaxLimitSendDataIA = model.MaxLimitSendDataIA,
            Price = model.Price,
            Title = model.Title,
        };
        AddChapterQuestions(model, plan);

        var addResult = await db.Plans.AddAsync(plan).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return addResult.Entity;
    }



    public async Task<Plan?> UpdateAsync(AddNewPlanRequest model, long loggedUserId, string loggedUserName)
    {
        //var validation = await model.ValidateUpdateAsync();
        //if (!validation.IsValid)
        //{
        //    notification.AddNotifications(validation);
        //    return default;
        //}
        var plan = await GetByIdAsync(model.Id).ConfigureAwait(false);
        if (plan != null)
        {
            plan.CaractersLimitFactor = model.CaractersLimitFactor;
            plan.Currency = model.Currency;
            plan.Price = model.Price;
            plan.Title = model.Title;
            plan.FinalValidityPeriod = model.FinalValidityPeriod;
            plan.MaxLimitSendDataIA = model.MaxLimitSendDataIA;
            plan.InitialValidityPeriod = model.InitialValidityPeriod;
            if (plan.PlanChapters.Any())
            {
                foreach (var pc in plan.PlanChapters)
                {
                    db.PlansChapters.Remove(pc);
                }
               
            }
            AddChapterQuestions(model, plan);
            plan.UpdatedAt = DateTimeBr.Now;
            plan.UpdatedBy = loggedUserName;
            plan.IsActive = true;
            plan.DeletedAt = null;
            db.Plans.Update(plan);
            await db.SaveChangesAsync().ConfigureAwait(false);
            return plan;
        }
        return null;
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

    public async Task<List<PlanChapter>?> GetPlanChapterByIdPlanAsync(long idPlan)
    {
       return await db.PlansChapters.Include(r=>r.Chapter).Where(r=>r.PlanId == idPlan).Include(r=>r.PlanChapterQuestions).ThenInclude(g=>g.Question).ToListAsync().ConfigureAwait(false);
    }
    private static void AddChapterQuestions(AddNewPlanRequest model, Plan plan)
    {
        if (model.ChapterPlanQuestion.Any())
        {
            var chapters = model.ChapterPlanQuestion.GroupBy(r => r.ChapterID);
            plan.PlanChapters = new List<PlanChapter>();
            foreach (var chapter in chapters)
            {
                var chapterId = chapter.Key;
                var planChapterQuestion = new List<PlanChapterQuestion>();
                foreach (var item in chapter)
                {
                    var questionId = item.QuestionId;
                    planChapterQuestion.Add(new PlanChapterQuestion()
                    {
                        QuestionId = questionId
                    });
                }
                plan.PlanChapters.Add(new PlanChapter()
                {
                    ChapterId = chapterId,
                    PlanChapterQuestions = planChapterQuestion
                });
            }
        }
    }
}
