namespace IAutor.Api.Services;

public interface IIncomeService
{
    Task<Income?> GetByIdAsync(long id);
    Task<List<Income>> GetAllAsync(IncomeFilters filters);
    Task<List<IncomeGroupedByDayDto>> GetGroupedByDayAsync(IncomeGroupedFilters filters);

}

public sealed class IncomeService(IAutorDb db, IIuguIntegrationService iuguIntegrationService) : IIncomeService
{
    public async Task<Income?> GetByIdAsync(long id) => await db.Incomes.FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);

    public async Task<List<Income>> GetAllAsync(IncomeFilters filters)
    {
        var predicate = PredicateBuilder.New<Income>(true);

        if (filters.Id.HasValue)
            predicate.And(a => a.Id == filters.Id);

        if (filters.CreatedAt.HasValue && filters.CreatedAt > DateTime.MinValue)
            predicate.And(a => a.CreatedAt == filters.CreatedAt.Value.Date);

        if (filters.UpdatedAt.HasValue && filters.UpdatedAt > DateTime.MinValue)
            predicate.And(a => a.UpdatedAt == filters.UpdatedAt.Value.Date);

        if (filters.OwnerId.HasValue)
            predicate.And(a => a.OwnerId == filters.OwnerId);

        if (filters.DateReference.HasValue && filters.DateReference > DateTime.MinValue)
            predicate.And(a => a.DateReference == filters.DateReference.Value);

        if (filters.SumValue.HasValue && filters.SumValue > decimal.Zero)
            predicate.And(a => a.SumValue == filters.SumValue);

        var query = db.Incomes.Where(predicate);

        if (filters.IncludeOwner.HasValue && filters.IncludeOwner.Value)
            query = query.Include(i => i.Owner);

        query = filters?.OrderBy switch
        {
            "1" => query.OrderBy(o => o.SumValue),
            "2" => query.OrderByDescending(o => o.SumValue),
            "3" => query.OrderBy(o => o.CreatedAt),
            "4" => query.OrderByDescending(o => o.CreatedAt),
            _ => query.OrderBy(o => o.Id)
        };

        return await query.AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<List<IncomeGroupedByDayDto>> GetGroupedByDayAsync(IncomeGroupedFilters filters)
    {
        #region Filters

        var qtdDays = filters?.QtdDays ?? 7;
        var dateStart = DateTimeBr.Date.AddDays(-(qtdDays - 1));
        var predicate = PredicateBuilder.New<Income>(a => a.DateReference >= dateStart);

        if (!string.IsNullOrEmpty(filters.Filter))
        {
            predicate.And(a =>
                EF.Functions.ILike(a.Owner.FirstName, filters.Filter.LikeConcat()) ||
                EF.Functions.ILike(a.Owner.LastName, filters.Filter.LikeConcat())
            );
        }

        if (filters != null && filters.OwnerId > 1)//Id 1 é da conta principal IAutor
            predicate.And(a => a.OwnerId == filters.OwnerId);

        #endregion

        var incomes = await db.Incomes.Where(predicate).AsNoTracking().ToListAsync().ConfigureAwait(false);
        if (incomes.Count == 0) return [];

        #region Owners

        var ownerIds = incomes.Select(s => s.OwnerId).GroupBy(g => g).Select(s => s.FirstOrDefault()).ToList();
        var owners = await db.Owners.Where(w => ownerIds.Contains(w.Id))
            .Select(s => new Owner
            {
                Id = s.Id,
                ProfileImgUrl = s.ProfileImgUrl,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Type = s.Type,
                IuguUserToken = s.IuguUserToken,
            })
            .ToListAsync().ConfigureAwait(false);

        if (string.IsNullOrEmpty(filters?.Filter) || ownerIds.Count == 1 || ownerIds.Contains(1))
            ownerIds.Add(0);//Adiciona id 0 para 2 primeiras linhas com valor de Volume de Venda

        #endregion

        #region Saldo Iugu no dia

        var ownerIdFilter = filters?.OwnerId ?? 1;

        if (!string.IsNullOrEmpty(filters?.Filter))
            ownerIdFilter = ownerIds.FirstOrDefault();

        var ownerFilter = owners.FirstOrDefault(f => f.Id == ownerIdFilter);
        var iuguFinancial = await iuguIntegrationService.GetFinancialBalanceByDayAsync(ownerFilter!, DateTimeBr.Date);

        #endregion

        var items = new List<IncomeGroupedByDayDto>(filters?.QtdDays ?? 7);

        foreach (var ownerId in ownerIds)
        {
            for (short l = 1; l < 3; l++)
            {
                var id = ownerId == 0 ? filters?.OwnerId ?? 1 : ownerId;

                if (ownerId == 0 && !string.IsNullOrEmpty(filters?.Filter))
                    id = ownerIds.FirstOrDefault();

                var owner = owners.FirstOrDefault(f => f.Id == id);
                var item = new IncomeGroupedByDayDto
                {
                    OwnerId = ownerId,
                    OwnerProfileImgUrl = l == 1 ? owner?.ProfileImgUrl ?? string.Empty : string.Empty,
                    OwnerName = owner?.Fullname ?? string.Empty,
                    OwnerType = owner?.Type ?? OwnerTypeEnum.Other,
                    Dates = new List<string>(7),
                    ValueIsMoney = ownerId > 0,
                    ValueAcomulated = l == 2,
                    Values = new List<decimal>(7)
                };

                if (id == ownerIdFilter)
                    item.IuguBalance = iuguFinancial?.InitialBalance?.Amount ?? "R$ 0,00";

                for (short d = 0; d < qtdDays; d++)
                {
                    var date = dateStart.AddDays(d);
                    var dateString = date.ToString("dd/MM");
                    item.Dates.Add(dateString);

                    if (item.ValueAcomulated == false)
                    {
                        var value = incomes.Where(f => f.OwnerId == id && f.DateReference == date.Date).Sum(s => s.SumValue);
                        item.Values.Add(value);

                        if (ownerId == 0)
                        {
                            var qtdValue = incomes.Where(f => f.OwnerId == id && f.DateReference == date.Date).Sum(s => s.SalesAmount);
                            item.QtdValues.Add(qtdValue);
                        }
                    }
                    else
                    {
                        var lastLine = items.LastOrDefault()!;
                        item.Values.Add(d == 0 ? lastLine.Values[d] : item.Values[d - 1] + lastLine.Values[d]);

                        if (ownerId == 0)
                            item.QtdValues.Add(d == 0 ? lastLine.QtdValues[d] : item.QtdValues[d - 1] + lastLine.QtdValues[d]);
                    }
                }

                item.Total = item.ValueAcomulated == false ? item.Values.Sum() : items.LastOrDefault()!.Values.Sum();
                item.QtdTotal = item.ValueAcomulated == false ? item.QtdValues.Sum() : items.LastOrDefault()!.QtdValues.Sum();

                items.Add(item);
            }
        }

        return [.. items.OrderBy(o => o.OwnerId == 0 ? -2 : o.OwnerId == 1 ? -1 : 0).ThenByDescending(t => t.Total)];
    }
}
