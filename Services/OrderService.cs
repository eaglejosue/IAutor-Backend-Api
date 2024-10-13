namespace IAutor.Api.Services;

public interface IOrderService
{
    Task<Order?> GetByIdAsync(long id);
    Task<List<Order>> GetAllAsync(OrderFilters filters);
    Task<Order?> CreateAsync(Order model, string userEmail);
    Task<Order?> UpdateAsync(Order model);
    Task<Order?> PatchAsync(Order model);
    Task<bool?> DeleteAsync(long id);
}

public sealed class OrderService(
    IAutorDb db,
    IBookService bookService,
    IPaymentService paymentService,
    IIuguIntegrationService iuguIntegrationService,
    INotificationService notification) : IOrderService
{
    public async Task<Order?> GetByIdAsync(long id) => await db.Orders.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<List<Order>> GetAllAsync(OrderFilters filters)
    {
        var predicate = PredicateBuilder.New<Order>(true);

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

        if (filters.UserId.HasValue)
            predicate.And(a => a.UserId == filters.UserId);

        if (filters.BookId.HasValue)
            predicate.And(a => a.BookId == filters.BookId);

        var query = db.Orders.Where(predicate);

        if (filters.IncludeUser.HasValue && filters.IncludeUser.Value)
            query = query.Include(i => i.User);

        if (filters.IncludeBook.HasValue && filters.IncludeBook.Value)
            query = query.Include(i => i.Book);

        if (filters.PaymentId.HasValue)
        {
            predicate.And(a => a.Payments.Any(a => a.Id == filters.PaymentId));

            if (filters.IncludePayment.HasValue && filters.IncludePayment.Value)
                query = query.Include(i => i.Payments.FirstOrDefault(f => f.Id == filters.PaymentId));
        }

        return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<Order?> CreateAsync(Order model, string userEmail)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var addResult = await db.Orders.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        var newOrder = addResult.Entity;

        var book = (await bookService.GetAllAsync(new BookFilters { Id = newOrder.BookId })).FirstOrDefault();
        if (book == null)
        {
            notification.AddNotification("Order", "Livro não encontrado.");
            return newOrder;
        }

        var fatura = await iuguIntegrationService.CreateFaturaAsync(userEmail, newOrder.Id, book.Title, book.Price);
        if (notification.HasNotifications) return newOrder;

        newOrder.IuguFaturaSecureUrl = fatura!.SecureUrl;

        await paymentService.CreateAsync(new Payment(newOrder, book!.Price, fatura));

        return newOrder;
    }

    public async Task<Order?> UpdateAsync(Order model)
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
            db.Orders.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        return entitie;
    }

    public async Task<Order?> PatchAsync(Order model)
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
            db.Orders.Update(entitie);
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

        db.Orders.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}
