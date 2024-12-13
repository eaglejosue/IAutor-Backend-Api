namespace IAutor.Api.Services.Crud;

public interface IBookService
{
    Task<Book?> GetByIdAsync(long id);
    Task<List<Book>> GetAllAsync(BookFilters filters, long? loggedUserId = null);
    Task<Book?> CreateAsync(Book model);
    Task<Book?> UpdateAsync(Book model, long loggedUserId, string loggedUserName);
    Task<Book?> PatchAsync(Book model, long loggedUserId, string loggedUserName);
    Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName);
    Task<PdfFileInfo?> GetBookPDF(long id);
}

public sealed class BookService(
    IAutorDb db,
    INotificationService notification,
    IUserService userService,
    IPDFService pdfService) : IBookService
{
    public async Task<Book?> GetByIdAsync(long id) => await db.Books.Where(w => w.Id == id).Include(i => i.QuestionUserAnswers).FirstOrDefaultAsync().ConfigureAwait(false);

    public async Task<List<Book>> GetAllAsync(BookFilters filters, long? loggedUserId = null)
    {
        var predicate = PredicateBuilder.New<Book>(true);

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
                EF.Functions.Like(a.Title, filters.Filter.LikeConcat()) ||
                EF.Functions.Like(a.Description, filters.Filter.LikeConcat())
            );
        }

        if (!string.IsNullOrEmpty(filters.CloudinaryPublicId))
            predicate.And(a => a.PublicId != null && EF.Functions.Like(a.PublicId, filters.CloudinaryPublicId.LikeConcat()));

        if (filters.Price.HasValue && filters.Price.Value > decimal.Zero)
            predicate.And(a => a.Price == filters.Price);

        if (filters.SaleExpirationDate.HasValue && filters.SaleExpirationDate > DateTime.MinValue)
            predicate.And(a => a.SaleExpirationDate == filters.SaleExpirationDate.Value.Date);

        if (filters.PromotionPrice.HasValue && filters.PromotionPrice.Value > decimal.Zero)
            predicate.And(a => a.PromotionPrice == filters.PromotionPrice);

        if (filters.PromotionExpirationDate.HasValue && filters.PromotionExpirationDate > DateTime.MinValue)
            predicate.And(a => a.PromotionExpirationDate == filters.PromotionExpirationDate.Value.Date);

        if (filters?.PaymentsApproved ?? false)
            predicate.And(a => a.Orders.Any(order =>
                order.Payments.Any(a => a.Status == PaymentStatus.Paid) &&
                order.UserId == loggedUserId
            ));

        var dateTimeNow = DateTimeBr.Now;

        if ((filters?.ListToDownload ?? false) == true)
            predicate.And(a => a.DownloadExpirationDate.HasValue && a.DownloadExpirationDate >= dateTimeNow || a.DownloadExpirationDate == null);
        else if ((filters?.ListToCrud ?? false) == false)
            predicate.And(a => a.SaleExpirationDate.HasValue && a.SaleExpirationDate >= dateTimeNow || a.SaleExpirationDate == null);

        if (filters.PlanId.HasValue)
            predicate.And(a => a.PlanId == filters.PlanId);

        if (filters.UserId.HasValue)
            predicate.And(a => a.UserId == filters.UserId);

        #endregion

        var query = db.Books.Where(predicate);

        #region Includes

        if (filters?.IncludeUserBookLogs ?? false)
            query = query.Include(i => i.UserBookLogs.Take(1));

        if (filters?.IncludeUserBookPlan ?? false)
            query = query.Include(i => i.Plan);

        if ((filters?.PaymentsApproved ?? false) || (filters?.IncludePayments ?? false))
            query = query
                .Include(i => i.Orders.Where(w => w.UserId == loggedUserId && w.Payments.Any(a => a.Status == PaymentStatus.Paid)).Take(1))
                .ThenInclude(t => t.Payments.Where(w => w.Status == PaymentStatus.Paid).Take(1));

        #endregion

        #region OrderBy

        query = filters?.OrderBy switch
        {
            "1" => query.OrderBy(o => o.Title),
            "2" => query.OrderByDescending(o => o.Title),
            "3" => query.OrderBy(o => o.CreatedAt),
            "4" => query.OrderByDescending(o => o.CreatedAt),
            _ => query.OrderBy(o => o.Id)
        };

        #endregion

#if DEBUG
        var queryString = query.ToQueryString();
#endif

        var items = await query.ToListAsync().ConfigureAwait(false);

        if (items.Any(a => !string.IsNullOrEmpty(a.PaidDateTime)))
            items = [.. items.OrderBy(o => string.IsNullOrEmpty(o.PaidDateTime))];

        return items;
    }

    public async Task<Book?> CreateAsync(Book model)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var addResult = await db.Books.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return addResult.Entity;
    }

    public async Task<Book?> UpdateAsync(Book model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidateUpdateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        return await UpdateEntityAsync(model, loggedUserId, loggedUserName, "Updated");
    }

    public async Task<Book?> PatchAsync(Book model, long loggedUserId, string loggedUserName)
    {
        var validation = await model.ValidatePatchAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        return await UpdateEntityAsync(model, loggedUserId, loggedUserName, "Patched");
    }

    private async Task<Book?> UpdateEntityAsync(Book model, long loggedUserId, string loggedUserName, string changeFrom)
    {
        var entitie = await db.Books.FirstOrDefaultAsync(f => f.Id == model.Id).ConfigureAwait(false);

        if (entitie == null) return null;

        if (entitie.EntityUpdated(model))
        {
            entitie.UpdatedAt = DateTimeBr.Now;
            entitie.UpdatedBy = loggedUserName;
            db.Books.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);

            await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Book Id {0} {1}", model.Id, changeFrom)));
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

        db.Books.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);

        await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Book Id {0} Deleted", id)));

        return true;
    }

    public async Task<PdfFileInfo?> GetBookPDF(long id)
    {
        var book = await db.Books.Where(w => w.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);

        if (book == null) return null;

        var query = db.PlansChapters
            .Where(w => w.PlanId == book.PlanId)
            .Include(pc => pc.Chapter)
            .Include(pc => pc.PlanChapterQuestions)
                .ThenInclude(pcq => pcq.Question)
                    .ThenInclude(q => q.QuestionUserAnswers);

        var chapters = await query
            .OrderBy(o => o.Chapter.ChapterNumber)
            .Select(s => new Chapter
            {
                Id = s.Chapter.Id,
                Title = s.Chapter.Title,
                ChapterNumber = s.Chapter.ChapterNumber,
                Questions = s.PlanChapterQuestions.Select(pcq => new Question
                {
                    Id = pcq.Question.Id,
                    Subject = pcq.Question.Subject,
                    QuestionUserAnswers = pcq.Question.QuestionUserAnswers.Where(w => w.BookId == book.Id).ToList()
                }).ToList(),
            })
            .ToListAsync()
            .ConfigureAwait(false);

        return await pdfService.GenerateBookPDF(book, chapters);
    }
}
