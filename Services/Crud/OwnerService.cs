namespace IAutor.Api.Services.Crud;

public interface IOwnerService
{
    Task<Owner?> GetByIdAsync(long id);
    Task<List<Owner>> GetAllAsync(OwnerFilters filters);
    Task<Owner?> CreateAsync(Owner model);
    Task<Owner?> UpdateAsync(Owner model, long loggedUserId, string loggedUserName);
    Task<Owner?> PatchAsync(Owner model, long loggedUserId, string loggedUserName);
    Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName);
    Task<Owner?> VerifyIuguSubAccountAsync(long id, Owner? owner = null);
}

public sealed class OwnerService(
    IAutorDb db,
    IUserService userService,
    IIuguIntegrationService iuguIntegrationService,
    INotificationService notification) : IOwnerService
{
    public async Task<Owner?> GetByIdAsync(long id) => await db.Owners.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<List<Owner>> GetAllAsync(OwnerFilters filters)
    {
        var predicate = PredicateBuilder.New<Owner>(true);

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
                EF.Functions.Like(a.FirstName, filters.Filter.LikeConcat()) ||
                EF.Functions.Like(a.LastName, filters.Filter.LikeConcat()) ||
                EF.Functions.Like(a.User.Email, filters.Filter.LikeConcat())
            );
        }

        if (!string.IsNullOrEmpty(filters.FirstName))
            predicate.And(a => a.FirstName != null && EF.Functions.Like(a.FirstName, filters.FirstName.LikeConcat()));

        if (!string.IsNullOrEmpty(filters.LastName))
            predicate.And(a => a.LastName != null && EF.Functions.Like(a.LastName, filters.LastName.LikeConcat()));

        if (filters?.IuguAccountVerified ?? false)
            predicate.And(a => a.IuguAccountVerified == true);

        #endregion

        var query = db.Owners.Where(predicate);

        #region Includes

        if (filters?.IncludeUserInfo ?? false)
            query = query.Include(i => i.User);

        #endregion

        #region OrderBy

        query = filters?.OrderBy switch
        {
            "1" => query.OrderBy(o => o.FirstName),
            "2" => query.OrderByDescending(o => o.FirstName),
            "3" => query.OrderBy(o => o.CreatedAt),
            "4" => query.OrderByDescending(o => o.CreatedAt),
            _ => query.OrderBy(o => o.Id)
        };

        #endregion

        #region Projection

        query = filters?.GetIdAndNameOnly ?? false
            ?
            query.Select(s => new Owner
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName
            })
            :
            query.Select(s => new Owner
            {
                Id = s.Id,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                DeletedAt = s.DeletedAt,
                UserId = s.UserId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                PersonType = s.PersonType,
                Cpf = s.Cpf,
                Cnpj = s.Cnpj,
                CnpjRespName = s.CnpjRespName,
                CnpjRespCpf = s.CnpjRespCpf,
                Address = s.Address,
                Cep = s.Cep,
                City = s.City,
                District = s.District,
                State = s.State,
                Telephone = s.Telephone,
                Bank = s.Bank,
                BankAg = s.BankAg,
                BankAccountNumber = s.BankAccountNumber,
                BankAccountType = s.BankAccountType,
                IuguAccountVerified = s.IuguAccountVerified,
                IuguAccountId = s.IuguAccountId,
                UpdatedBy = s.UpdatedBy,
                User = new User
                {
                    Id = s.UserId,
                    Email = s.User.Email,
                    ProfileImgUrl = s.User.ProfileImgUrl,
                    BirthDate = s.User.BirthDate
                },
            });

        #endregion

#if DEBUG
        var queryString = query.ToQueryString();
#endif

        return await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<Owner?> CreateAsync(Owner model)
    {
        var validation = await model.ValidateCreateAsync();
        if (!validation.IsValid)
        {
            notification.AddNotifications(validation);
            return default;
        }

        var ownerWithBankData = await db.Owners
            .AnyAsync(a => a.IsActive &&
                a.Bank == model.Bank &&
                a.BankAg == model.BankAg &&
                a.BankAccountType == model.BankAccountType &&
                a.BankAccountNumber == model.BankAccountNumber
            ).ConfigureAwait(false);

        if (ownerWithBankData)
        {
            notification.AddNotification("Owner", "Já existe um participante cadastrado com os mesmos dados bancários.");
            return default;
        }

        var user = await db.Users.FirstOrDefaultAsync(f => f.Email == model.Email);
        if (user == null)
            user = await userService.CreateAsync(new User(model));

        model.UserId = user!.Id;

        var subAccount = await iuguIntegrationService.CreateSubAccountAsync(user.Fullname);
        if (notification.HasNotifications) return default;

        model.SetIuguTokens(subAccount!);

        var addResult = await db.Owners.AddAsync(model).ConfigureAwait(false);
        await db.SaveChangesAsync().ConfigureAwait(false);

        var newOwner = addResult.Entity;

        await VerifyIuguSubAccountAsync(newOwner.Id, newOwner);

        return newOwner;
    }

    public async Task<Owner?> UpdateAsync(Owner model, long loggedUserId, string loggedUserName)
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
            entitie.UpdatedBy = loggedUserName;
            db.Owners.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);

            await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Owner Id {0} Patched", model.Id)));
        }

        return entitie;
    }

    public async Task<Owner?> PatchAsync(Owner model, long loggedUserId, string loggedUserName)
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
            entitie.UpdatedBy = loggedUserName;
            db.Owners.Update(entitie);
            await db.SaveChangesAsync().ConfigureAwait(false);

            await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Owner Id {0} Patched", model.Id)));
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

        db.Owners.Update(entitie);
        await db.SaveChangesAsync().ConfigureAwait(false);

        await userService.CreateUserLogAsync(new UserLog(loggedUserId, string.Format("Owner Id {0} Deleted", id)));

        return true;
    }

    public async Task<Owner?> VerifyIuguSubAccountAsync(long id, Owner? owner = null)
    {
        if (owner == null)
            owner = await GetByIdAsync(id);

        if (owner == null)
        {
            notification.AddNotification("Owner", "Dados de integração com a Iugu não cadastrados.");
            return default;
        }

        if (owner.IuguAccountId is null || owner.IuguUserToken is null)
        {
            notification.AddNotification("Owner", "Dados de integração com a Iugu não cadastrados.");
            return default;
        }

        var iuguResult = await iuguIntegrationService.VerifySubAccountAsync(owner);

        if (notification.HasNotifications)
        {
            if (iuguResult != null)
            {
                notification.ClearNotifications();

                if (iuguResult.Errors.Agency != null)
                    notification.AddNotifications(iuguResult.Errors.Agency.Select(a => new Notification("Agency", a)).ToList());

                if (iuguResult.Errors.Account != null)
                    notification.AddNotifications(iuguResult.Errors.Account.Select(a => new Notification("Número da Conta", a)).ToList());
            }

            return owner;
        }

        owner.IuguAccountVerified = true;
        owner.UpdatedAt = DateTimeBr.Now;
        db.Owners.Update(owner);
        await db.SaveChangesAsync().ConfigureAwait(false);

        return owner;
    }
}
