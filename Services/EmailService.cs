namespace IAutor.Api.Services;

public interface IEmailService
{
    Task<Email?> GetByIdAsync(long id);
    Task<List<Email>> GetAllAsync(EmailFilters filters);
    Task<Email?> CreateAsync(Email model);
    Task<Email?> UpdateAsync(Email model);
    Task<Email?> PatchAsync(Email model);
    Task<bool?> DeleteAsync(long id);
}

public sealed class EmailService(
    IAutorDb db,
    INotificationService notification) : IEmailService
{
    public async Task<Email?> GetByIdAsync(long id) => await db.Emails.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<List<Email>> GetAllAsync(EmailFilters filters)
    {
        var predicate = PredicateBuilder.New<Email>(true);

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

        if (filters.EmailType.HasValue)
            predicate.And(a => a.EmailType == filters.EmailType);

        if (filters.ScheduleDate.HasValue && filters.ScheduleDate > DateTime.MinValue)
            predicate.And(a => a.ScheduleDate == filters.ScheduleDate.Value.Date);

        if (filters.DateSent.HasValue && filters.DateSent > DateTime.MinValue)
            predicate.And(a => a.DateSent == filters.DateSent.Value.Date);

        if (filters.SendAttempts.HasValue)
            predicate.And(a => a.SendAttempts == filters.SendAttempts);

        var query = db.Emails.Where(predicate);

        if (filters.IncludeUser.HasValue && filters.IncludeUser.Value)
            query = query.Include(i => i.User);

        return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<Email?> CreateAsync(Email model)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var addResult = await db.Emails.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return addResult.Entity;
    }

    public async Task<Email?> UpdateAsync(Email model)
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
            db.Emails.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        return entitie;
    }

    public async Task<Email?> PatchAsync(Email model)
    {
        var validation = await model.ValidatePatchAsync();
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
            db.Emails.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        return entitie;
    }

    public async Task<bool?> DeleteAsync(long id)
    {
        var entitie = await GetByIdAsync(id);
        if (entitie == null) return null;

        entitie.IsActive = false;
        entitie.DeletedAt = DateTimeBr.Now;

        db.Emails.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}
