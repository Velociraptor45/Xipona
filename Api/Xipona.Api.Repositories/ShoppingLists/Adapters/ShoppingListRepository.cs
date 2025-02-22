using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;
using Discount = ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.Discount;
using ShoppingList = ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities.ShoppingList;

namespace ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Adapters;

public class ShoppingListRepository : IShoppingListRepository
{
    private readonly ShoppingListContext _dbContext;
    private readonly IToDomainConverter<ShoppingList, IShoppingList> _toDomainConverter;
    private readonly IToContractConverter<IShoppingList, ShoppingList> _toContractConverter;
    private readonly ILogger<ShoppingListRepository> _logger;
    private readonly CancellationToken _cancellationToken;

    public ShoppingListRepository(ShoppingListContext dbContext,
        IToDomainConverter<ShoppingList, IShoppingList> toDomainConverter,
        IToContractConverter<IShoppingList, ShoppingList> toContractConverter,
        ILogger<ShoppingListRepository> logger,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toContractConverter = toContractConverter;
        _logger = logger;
        _cancellationToken = cancellationToken;
    }

    #region public methods

    public async Task StoreAsync(IShoppingList shoppingList)
    {
        var listEntity = await FindEntityByIdAsync(shoppingList.Id);

        if (listEntity is null)
        {
            await StoreAsNewListAsync(shoppingList);
            return;
        }

        await StoreModifiedListAsync(listEntity, shoppingList);

        _dbContext.ChangeTracker.Clear();
    }

    public async Task<IShoppingList?> FindByAsync(ShoppingListId id)
    {
        var entity = await GetShoppingListQuery()
            .FirstOrDefaultAsync(list => list.Id == id, _cancellationToken);

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IShoppingList>> FindByAsync(ItemId itemId)
    {
        List<ShoppingList> entities = await GetShoppingListQuery()
            .Where(l => l.ItemsOnList.Any(i => i.ItemId == itemId))
            .ToListAsync(cancellationToken: _cancellationToken);

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IShoppingList>> FindByAsync(ItemTypeId typeId)
    {
        var entities = await GetShoppingListQuery()
            .Where(l => l.ItemsOnList.Any(i => i.ItemTypeId.HasValue && i.ItemTypeId == typeId))
            .ToListAsync(_cancellationToken);

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IShoppingList>> FindActiveByAsync(ItemId itemId)
    {
        List<ShoppingList> entities = await GetShoppingListQuery()
            .Where(l => l.ItemsOnList.FirstOrDefault(i => i.ItemId == itemId) != null
                        && l.CompletionDate == null)
            .ToListAsync(cancellationToken: _cancellationToken);

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IShoppingList?> FindActiveByAsync(StoreId storeId)
    {
        var entity = await GetShoppingListQuery()
            .FirstOrDefaultAsync(list =>
                    list.CompletionDate == null
                    && list.StoreId == storeId,
                _cancellationToken);

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IShoppingList>> FindActiveByAsync(IEnumerable<StoreId> storeIds)
    {
        var rawStoreIds = storeIds.Select(s => s.Value).ToList();
        var entities = await GetShoppingListQuery()
            .Where(list =>
                    list.CompletionDate == null
                    && rawStoreIds.Contains(list.StoreId))
            .ToListAsync(_cancellationToken);

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task DeleteAsync(ShoppingListId id)
    {
        var entity = await FindEntityByIdAsync(id);
        if (entity == null)
            return;

        _dbContext.ShoppingLists.Remove(entity);
        await _dbContext.SaveChangesAsync(_cancellationToken);
    }

    #endregion public methods

    #region private methods

    private async Task StoreModifiedListAsync(ShoppingList existingEntity,
        IShoppingList shoppingList)
    {
        var updatedEntity = _toContractConverter.ToContract(shoppingList);
        var onListMappings = existingEntity.ItemsOnList.ToDictionary(map => (map.ItemId, map.ItemTypeId));
        var onListDiscounts = existingEntity.Discounts.ToDictionary(map => (map.ItemId, map.ItemTypeId));

        var existingRowVersion = existingEntity.RowVersion;
        _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
        _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
        _dbContext.Entry(existingEntity).State = EntityState.Modified;

        StoreMapping(updatedEntity, onListMappings);
        StoreDiscounts(updatedEntity, onListDiscounts);

        try
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex,
                "Saving shopping list {ShoppingListId} failed due to concurrency violation", shoppingList.Id.Value);
            throw new DomainException(new ModelOutOfDateReason());
        }
    }

    private void StoreMapping(ShoppingList updatedEntity,
        Dictionary<(Guid ItemId, Guid? ItemTypeId), ItemsOnList> onListMappings)
    {
        foreach (var map in updatedEntity.ItemsOnList)
        {
            if (onListMappings.TryGetValue((map.ItemId, map.ItemTypeId), out var existingMapping))
            {
                // mapping was modified
                map.Id = existingMapping.Id;
                _dbContext.Entry(map).State = EntityState.Modified;
                onListMappings.Remove((map.ItemId, map.ItemTypeId));
            }
            else
            {
                // mapping was added
                _dbContext.Entry(map).State = EntityState.Added;
            }
        }

        // mapping was deleted
        foreach (var map in onListMappings.Values)
        {
            _dbContext.Entry(map).State = EntityState.Deleted;
        }
    }

    private void StoreDiscounts(ShoppingList updatedEntity,
        Dictionary<(Guid ItemId, Guid? ItemTypeId), Discount> onListDiscounts)
    {
        foreach (var discount in updatedEntity.Discounts)
        {
            if (onListDiscounts.TryGetValue((discount.ItemId, discount.ItemTypeId), out var existingDiscount))
            {
                // mapping was modified
                discount.Id = existingDiscount.Id;
                _dbContext.Entry(discount).State = EntityState.Modified;
                onListDiscounts.Remove((discount.ItemId, discount.ItemTypeId));
            }
            else
            {
                // mapping was added
                _dbContext.Entry(discount).State = EntityState.Added;
            }
        }

        // mapping was deleted
        foreach (var discount in onListDiscounts.Values)
        {
            _dbContext.Entry(discount).State = EntityState.Deleted;
        }
    }

    private async Task StoreAsNewListAsync(IShoppingList shoppingList)
    {
        var entity = _toContractConverter.ToContract(shoppingList);

        _dbContext.Entry(entity).State = EntityState.Added;
        foreach (var onListMap in entity.ItemsOnList)
        {
            _dbContext.Entry(onListMap).State = EntityState.Added;
        }
        foreach (var onListDiscount in entity.Discounts)
        {
            _dbContext.Entry(onListDiscount).State = EntityState.Added;
        }

        await _dbContext.SaveChangesAsync(_cancellationToken);
    }

    private async Task<ShoppingList?> FindEntityByIdAsync(ShoppingListId id)
    {
        return await GetShoppingListQuery()
            .FirstOrDefaultAsync(list => list.Id == id, _cancellationToken);
    }

    private IQueryable<ShoppingList> GetShoppingListQuery()
    {
        return _dbContext.ShoppingLists.AsNoTracking()
            .Include(l => l.ItemsOnList)
            .Include(l => l.Discounts);
    }

    #endregion private methods
}