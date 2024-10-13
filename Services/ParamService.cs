namespace IAutor.Api.Services;

public interface IParamService
{
    Task<Param?> GetByKeyAsync(string key);
    Task<List<Param>> GetAllAsync(ParamFilters filters);
    Task<Param?> CreateAsync(Param model);
    Task<Param?> UpdateAsync(Param model);
    Task<Param?> PatchAsync(Param model);
    Task<bool?> DeleteAsync(string key);
}

public sealed class ParamService(
    IAutorDb db,
    INotificationService notification) : IParamService
{
    public async Task<Param?> GetByKeyAsync(string key) => await db.Params.FirstOrDefaultAsync(f => f.Key == key).ConfigureAwait(false);

    public async Task<List<Param>> GetAllAsync(ParamFilters filters)
    {
        var predicate = PredicateBuilder.New<Param>(true);

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

        if (!string.IsNullOrEmpty(filters.Key))
            predicate.And(a => EF.Functions.Like(a.Key, filters.Key.LikeConcat()));

        if (!string.IsNullOrEmpty(filters.Value))
            predicate.And(a => EF.Functions.Like(a.Value, filters.Value.LikeConcat()));

        return await db.Params.Where(predicate).AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<Param?> CreateAsync(Param model)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var addResult = await db.Params.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return addResult.Entity;
    }

    public async Task<Param?> UpdateAsync(Param model)
    {
        var validation = await model.ValidateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var entitie = await GetByKeyAsync(model.Key);
        if (entitie == null) return null;

        if (entitie.EntityUpdated(model))
        {
            entitie.UpdatedAt = DateTimeBr.Now;
            db.Params.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        return entitie;
    }

    public async Task<Param?> PatchAsync(Param model)
    {
        var validation = await model.ValidateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var entitie = await GetByKeyAsync(model.Key);
        if (entitie == null) return null;

        if (entitie.EntityUpdated(model))
        {
            entitie.UpdatedAt = DateTimeBr.Now;
            db.Params.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        return entitie;
    }

    public async Task<bool?> DeleteAsync(string key)
    {
        var entitie = await GetByKeyAsync(key);
        if (entitie == null) return null;

        entitie.IsActive = false;
        entitie.DeletedAt = DateTimeBr.Now;

        db.Params.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}
