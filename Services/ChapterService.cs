using IAutor.Api.Services.Http;

namespace IAutor.Api.Services;


public interface IChapterService
{
    Task<Chapter?> GetByIdAsync(long id);
    Task<List<Chapter>> GetAllAsync(ChapterFilters filters);
    Task<Chapter?> CreateAsync(Chapter model);
    Task<Chapter?> UpdateAsync(Chapter model, long loggedUserId, string loggedUserName);
    Task<Chapter?> PatchAsync(Chapter model, long loggedUserId, string loggedUserName);
    Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName);
}
public sealed class ChapterService(
    IAutorDb db,
    INotificationService notification) : IChapterService
{
    public async Task<Chapter?> CreateAsync(Chapter model)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var addResult = await db.Chapters.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        var newEntitie = addResult.Entity;

        return newEntitie;
    }

    public async Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName)
    {
        var entitie = await GetByIdAsync(id);
        if (entitie == null) return null;

        entitie.IsActive = false;
        entitie.DeletedAt = DateTimeBr.Now;

        db.Chapters.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);


        return true;
    }

    public async Task<List<Chapter>> GetAllAsync(ChapterFilters filters)
    {
        var predicate = PredicateBuilder.New<Chapter>(n => n.Id > 0);

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

        var query = db.Chapters.Where(predicate);

        #region OrderBy

        query = filters?.OrderBy switch
        {
            "1" => query.OrderBy(o => o.Title),
            "2" => query.OrderByDescending(o => o.ChapterNumber),
            "3" => query.OrderBy(o => o.CreatedAt),
            "4" => query.OrderByDescending(o => o.CreatedAt),
            _ => query.OrderBy(o => o.Id)
        };

        #endregion

#if DEBUG
        var queryString = query.ToQueryString();
#endif

        return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<Chapter?> GetByIdAsync(long id) => await db.Chapters.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<Chapter?> PatchAsync(Chapter model, long loggedUserId, string loggedUserName)
    {
        throw new NotImplementedException();
    }

    public async Task<Chapter?> UpdateAsync(Chapter model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidateUpdateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var entitie = await GetByIdAsync(model.Id);
        if (entitie == null) return null;

        if (entitie.EntityUpdated(model))
        {
            entitie.UpdatedAt = DateTimeBr.Now;
            db.Chapters.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        return entitie;
    }
}
