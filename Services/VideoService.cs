namespace Pay4Tru.Api.Services;

public interface IVideoService
{
    Task<Video?> GetByIdAsync(long id);
    Task<List<Video>> GetAllAsync(VideoFilters filters, long? loggedUserId = null);
    Task<Video?> CreateAsync(Video model);
    Task<Video?> UpdateAsync(Video model, long loggedUserId, string loggedUserName);
    Task<Video?> PatchAsync(Video model, long loggedUserId, string loggedUserName);
    Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName);
    Task<VideoTrailer?> GetVideoTrailerByIdAsync(long id);
    Task<List<VideoTrailer>> GetVideoTrailersByVideoIdAsync(long id);
    Task<VideoTrailer?> AddVideoTrailerAsync(VideoTrailer model);
    Task<bool?> RemoveVideoTrailerAsync(long id);
}

public sealed class VideoService(
    Pay4TruDb db,
    INotificationService notification,
    IUserService userService) : IVideoService
{
    public async Task<Video?> GetByIdAsync(long id) => await db.Videos.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<List<Video>> GetAllAsync(VideoFilters filters, long? loggedUserId = null)
    {
        var predicate = PredicateBuilder.New<Video>(true);

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

        if (!string.IsNullOrEmpty(filters.Filter))
        {
            predicate.And(a =>
                EF.Functions.ILike(a.Title, filters.Filter.LikeConcat()) ||
                EF.Functions.ILike(a.Description, filters.Filter.LikeConcat()) ||
                a.OwnerVideos.Any(a =>
                    EF.Functions.ILike(a.Owner.FirstName, filters.Filter.LikeConcat()) ||
                    EF.Functions.ILike(a.Owner.LastName, filters.Filter.LikeConcat())
                )
            );
        }

        if (!string.IsNullOrEmpty(filters.CloudinaryPublicId))
            predicate.And(a => a.CloudinaryPublicId != null && EF.Functions.ILike(a.CloudinaryPublicId, filters.CloudinaryPublicId.LikeConcat()));

        if (filters.ReleaseDate.HasValue && filters.ReleaseDate > DateTime.MinValue)
            predicate.And(a => a.ReleaseDate == filters.ReleaseDate.Value.Date);

        if (filters.Price.HasValue && filters.Price.Value > decimal.Zero)
            predicate.And(a => a.Price == filters.Price);

        if (filters.SaleExpirationDate.HasValue && filters.SaleExpirationDate > DateTime.MinValue)
            predicate.And(a => a.SaleExpirationDate == filters.SaleExpirationDate.Value.Date);

        if (filters.PromotionPrice.HasValue && filters.PromotionPrice.Value > decimal.Zero)
            predicate.And(a => a.PromotionPrice == filters.PromotionPrice);

        if (filters.PromotionExpirationDate.HasValue && filters.PromotionExpirationDate > DateTime.MinValue)
            predicate.And(a => a.PromotionExpirationDate == filters.PromotionExpirationDate.Value.Date);

        if (filters.WatchExpirationDate.HasValue && filters.WatchExpirationDate > DateTime.MinValue)
            predicate.And(a => a.WatchExpirationDate == filters.WatchExpirationDate.Value.Date);

        if (filters.OwnerId.HasValue)
            predicate.And(a => a.OwnerVideos.Any(a => a.OwnerId == filters.OwnerId));

        if (filters.TrailerId.HasValue)
            predicate.And(a => a.VideoTrailers.Any(a => a.Id == filters.TrailerId));

        if (filters?.PaymentsApproved ?? false)
            predicate.And(a => a.Orders.Any(order =>
                order.Payments.Any(a => a.Status == PaymentStatusEnum.Paid) &&
                order.UserId == loggedUserId
            ));

        var dateTimeNow = DateTimeBr.Now;

        if ((filters?.ListToWatch ?? false) == true)
            predicate.And(a => a.WatchExpirationDate.HasValue && a.WatchExpirationDate >= dateTimeNow || a.WatchExpirationDate == null);
        else if ((filters?.ListToCrud ?? false) == false)
            predicate.And(a => a.SaleExpirationDate.HasValue && a.SaleExpirationDate >= dateTimeNow || a.SaleExpirationDate == null);

        //if (filters?.ListCopies != null && filters.ListCopies == false)
        //    predicate.And(a => a.CopyFromVideoId == null);

        #endregion

        var query = db.Videos.Where(predicate);

        #region Includes

        if (filters?.IncludeOwnerVideos ?? false)
            query = query.Include(i => i.OwnerVideos);

        if (filters?.IncludeTrailers ?? false)
            query = query.Include(i => i.VideoTrailers);

        if (filters?.IncludeUserVideoLogs ?? false)
            query = query.Include(i => i.UserVideoLogs.Take(1));

        if ((filters?.PaymentsApproved ?? false) || (filters?.IncludePayments ?? false))
            query = query
                .Include(i => i.Orders.Where(w => w.UserId == loggedUserId && w.Payments.Any(a => a.Status == PaymentStatusEnum.Paid)).Take(1))
                .ThenInclude(t => t.Payments.Where(w => w.Status == PaymentStatusEnum.Paid).Take(1));

        #endregion

        #region OrderBy

        query = filters?.OrderBy switch
        {
            "1" => query.OrderBy(o => o.ReleaseDate),
            "2" => query.OrderByDescending(o => o.ReleaseDate),
            "3" => query.OrderBy(o => o.Title),
            "4" => query.OrderByDescending(o => o.Title),
            "5" => query.OrderBy(o => o.CreatedAt),
            "6" => query.OrderByDescending(o => o.CreatedAt),
            _ => query.OrderBy(o => o.Id)
        };

        #endregion

#if DEBUG
        var queryString = query.ToQueryString();
#endif

        var items = await query.AsNoTracking().ToListAsync().ConfigureAwait(false);

        if (items.Any(a => !string.IsNullOrEmpty(a.PaidDateTime)))
            items = [.. items.OrderBy(o => string.IsNullOrEmpty(o.PaidDateTime))];

        return items;
    }

    public async Task<Video?> CreateAsync(Video model)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        if (model.VideoTrailers != null)
        {
            model.VideoTrailers = model.VideoTrailers.Where(w => !string.IsNullOrEmpty(w.CloudinaryPublicId)).ToList();
            foreach (var item in model.VideoTrailers)
            {
                item.IsActive = true;
                if (item.Id == 0) item.CreatedAt = DateTimeBr.Now;
            }
        }

        if (model.OwnerVideos != null)
        {
            model.OwnerVideos = model.OwnerVideos.Where(w => w.OwnerId > 0 && w.PercentageSplit > 0).ToList();
            foreach (var item in model.OwnerVideos)
            {
                item.IsActive = true;
                if (item.Id == 0) item.CreatedAt = DateTimeBr.Now;
            }
        }

        var addResult = await db.Videos.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return addResult.Entity;
    }

    public async Task<Video?> UpdateAsync(Video model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidateUpdateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        return await UpdateEntityAsync(model, loggedUserId, loggedUserName, "Updated");
    }

    public async Task<Video?> PatchAsync(Video model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidatePatchAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        return await UpdateEntityAsync(model, loggedUserId, loggedUserName, "Patched");
    }

    private async Task<Video?> UpdateEntityAsync(Video model, long loggedUserId, string loggedUserName, string changeFrom)
    {
        var entitie = await db.Videos
            .Include(i => i.VideoTrailers)
            .Include(i => i.OwnerVideos)
            .FirstOrDefaultAsync(f => f.Id == model.Id).ConfigureAwait(false);

        if (entitie == null) return null;

        if (entitie.EntityUpdated(model))
        {
            if (model.VideoTrailers != null)
            {
                model.VideoTrailers = model.VideoTrailers.Where(w => !string.IsNullOrEmpty(w.CloudinaryPublicId)).ToList();
                foreach (var item in model.VideoTrailers)
                {
                    item.IsActive = true;
                    if (item.Id == 0) item.CreatedAt = DateTimeBr.Now;
                }
            }

            if (model.OwnerVideos != null)
            {
                model.OwnerVideos = model.OwnerVideos.Where(w => w.OwnerId > 0 && w.PercentageSplit > 0).ToList();
                foreach (var item in model.OwnerVideos)
                {
                    item.IsActive = true;
                    if (item.Id == 0) item.CreatedAt = DateTimeBr.Now;
                }
            }

            entitie.UpdatedAt = DateTimeBr.Now;
            entitie.UpdatedBy = loggedUserName;
            db.Videos.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);

            await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Video Id {0} {1}", model.Id, changeFrom)));
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

        db.Videos.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);
        
        await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Video Id {0} Deleted", id)));

        return true;
    }

    public async Task<VideoTrailer?> GetVideoTrailerByIdAsync(long id) =>
        await db.VideoTrailers.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<List<VideoTrailer>> GetVideoTrailersByVideoIdAsync(long id) =>
        await db.VideoTrailers.Where(w => w.VideoId == id).ToListAsync().ConfigureAwait(false);

    public async Task<VideoTrailer?> AddVideoTrailerAsync(VideoTrailer model)
    {
        var validation = await model.ValidateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        if (model.Id > 0)
        {
            var entitie = await db.VideoTrailers.FirstOrDefaultAsync(f => f.Id == model.Id).ConfigureAwait(false);
            if (entitie != null)
            {
                entitie.Title = model.Title;
                entitie.CloudinaryPublicId = model.CloudinaryPublicId;

                db.VideoTrailers.Update(entitie);
                await db.SaveChangesAsync().ConfigureAwait(false);

                return entitie;
            }
        }
        
        var addResult = await db.VideoTrailers.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);
        
        return addResult.Entity;
    }

    public async Task<bool?> RemoveVideoTrailerAsync(long id)
    {
        var entitie = await db.VideoTrailers.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

        if (entitie == null)
        {
            notification.AddNotification("VideoTrailer", "Registro não encontrado.");
            return default;
        }

        entitie.Inactivate();

        db.VideoTrailers.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}
