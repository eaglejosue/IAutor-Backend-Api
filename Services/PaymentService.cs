namespace IAutor.Api.Services;

public interface IPaymentService
{
    Task<Payment?> GetByIdAsync(long id);
    Task<List<Payment>> GetAllAsync(PaymentFilters filters);
    Task<Payment?> CreateAsync(Payment model);
    Task<Payment?> UpdateAsync(Payment model);
    Task<Payment?> PatchAsync(Payment model);
    Task<bool?> DeleteAsync(long id);
    Task<Payment?> UpdateStatusAsync(IuguPaymentChangedStatus model);
}

public sealed class PaymentService(
    IAutorDb db,
    IEmailService emailService,
    INotificationService notification,
    ILogger<PaymentService> logger) : IPaymentService
{
    public async Task<Payment?> GetByIdAsync(long id) => await db.Payments.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<List<Payment>> GetAllAsync(PaymentFilters filters)
    {
        var predicate = PredicateBuilder.New<Payment>(true);

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

        if (filters.OrderId.HasValue)
            predicate.And(a => a.OrderId == filters.OrderId);

        if (filters.PricePaid.HasValue && filters.PricePaid > decimal.Zero)
            predicate.And(a => a.PricePaid == filters.PricePaid);

        if (filters.PaymentStatus.HasValue)
            predicate.And(a => a.Status == filters.PaymentStatus);

        if (filters.PaymentDate.HasValue && filters.PaymentDate > DateTime.MinValue)
            predicate.And(a => a.IuguPaidAt != null && EF.Functions.Like(a.IuguPaidAt, filters.PaymentDate.Value.ToString().LikeConcat()));

        var query = db.Payments.Where(predicate).OrderBy(o => o.Id);

        return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<Payment?> CreateAsync(Payment model)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var entitie = await db.Payments.FirstOrDefaultAsync(
            f => f.OrderId == model.OrderId &&
                 f.IuguFaturaId == model.IuguFaturaId
            )
            .ConfigureAwait(false);

        if (entitie != null)
        {
            notification.AddNotification("Payment", "Pagamento existente.");
            return default;
        }

        model.CreatedAt = DateTimeBr.Now;
        var addResult = await db.Payments.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return addResult.Entity;
    }

    public async Task<Payment?> UpdateAsync(Payment model)
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
            db.Payments.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        return entitie;
    }

    public async Task<Payment?> PatchAsync(Payment model)
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
            db.Payments.Update(entitie);
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

        db.Payments.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    public async Task<Payment?> UpdateStatusAsync(IuguPaymentChangedStatus model)
    {
        logger.LogInformation("Payment - UpdateStatusAsync | IuguPaymentChangedStatus: {M}", JsonSerializer.Serialize(model));

        var payment = await db.Payments.Where(
            f => f.IuguOrderId == model.OrderId &&
                 f.IuguFaturaId == model.FaturaId
            )
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (payment == null) return null;

        var newEntity = new Payment(payment);
        newEntity.SetIuguData(model);

        var addResult = await db.Payments.AddAsync(newEntity).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        if (newEntity.Status == PaymentStatus.Paid)
        {
            var order = await db.Orders.FirstOrDefaultAsync(f => f.Id == newEntity.OrderId);

            var newEmail = await emailService.CreateAsync(new Email(
                order!.UserId,
                EmailType.BookReleaseSchedule,
                null,
                order!.BookId
            ));

            logger.LogInformation("Payment - UpdateStatusAsync | Book Released, calling Email API");
            //await emailService.SendBookReleaseAsync(newEmail!.Id);
        }

        return addResult.Entity;
    }
}
