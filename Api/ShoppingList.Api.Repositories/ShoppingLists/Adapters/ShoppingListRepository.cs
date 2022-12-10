using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Contexts;

namespace ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Adapters;

public class ShoppingListRepository : IShoppingListRepository
{
    private readonly ShoppingListContext _dbContext;
    private readonly IToDomainConverter<Entities.ShoppingList, IShoppingList> _toDomainConverter;
    private readonly IToEntityConverter<IShoppingList, Entities.ShoppingList> _toEntityConverter;

    public ShoppingListRepository(ShoppingListContext dbContext,
        IToDomainConverter<Entities.ShoppingList, IShoppingList> toDomainConverter,
        IToEntityConverter<IShoppingList, Entities.ShoppingList> toEntityConverter)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toEntityConverter = toEntityConverter;
    }

    #region public methods

    public async Task StoreAsync(IShoppingList shoppingList, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var listEntity = await FindEntityByIdAsync(shoppingList.Id);

        cancellationToken.ThrowIfCancellationRequested();

        if (listEntity is null)
        {
            await StoreAsNewListAsync(shoppingList, cancellationToken);
            return;
        }

        await StoreModifiedListAsync(listEntity, shoppingList, cancellationToken);

        _dbContext.ChangeTracker.Clear();
    }

    public async Task<IShoppingList?> FindByAsync(ShoppingListId id, CancellationToken cancellationToken)
    {
        var entity = await GetShoppingListQuery()
            .FirstOrDefaultAsync(list => list.Id == id.Value, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IShoppingList>> FindByAsync(ItemId itemId,
        CancellationToken cancellationToken)
    {
        List<Entities.ShoppingList> entities = await GetShoppingListQuery()
            .Where(l => l.ItemsOnList.Any(i => i.ItemId == itemId))
            .ToListAsync(cancellationToken: cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IShoppingList>> FindByAsync(ItemTypeId typeId,
        CancellationToken cancellationToken)
    {
        var entities = await GetShoppingListQuery()
            .Where(l => l.ItemsOnList.Any(i => i.ItemTypeId.HasValue && i.ItemTypeId == typeId))
            .ToListAsync(cancellationToken);

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IShoppingList>> FindActiveByAsync(ItemId itemId,
        CancellationToken cancellationToken)
    {
        List<Entities.ShoppingList> entities = await GetShoppingListQuery()
            .Where(l => l.ItemsOnList.FirstOrDefault(i => i.ItemId == itemId) != null
                        && l.CompletionDate == null)
            .ToListAsync(cancellationToken: cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IShoppingList?> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken)
    {
        var entity = await GetShoppingListQuery()
            .FirstOrDefaultAsync(list =>
                    list.CompletionDate == null
                    && list.StoreId == storeId.Value,
                cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    #endregion public methods

    #region private methods

    private async Task StoreModifiedListAsync(Entities.ShoppingList existingShoppingListEntity,
        IShoppingList shoppingList, CancellationToken cancellationToken)
    {
        var shoppingListEntityToStore = _toEntityConverter.ToEntity(shoppingList);
        var onListMappings = existingShoppingListEntity.ItemsOnList.ToDictionary(map => (map.ItemId, map.ItemTypeId));

        _dbContext.Entry(shoppingListEntityToStore).State = EntityState.Modified;
        foreach (var map in shoppingListEntityToStore.ItemsOnList)
        {
            cancellationToken.ThrowIfCancellationRequested();
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

        cancellationToken.ThrowIfCancellationRequested();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task StoreAsNewListAsync(IShoppingList shoppingList, CancellationToken cancellationToken)
    {
        var entity = _toEntityConverter.ToEntity(shoppingList);

        _dbContext.Entry(entity).State = EntityState.Added;
        foreach (var onListMap in entity.ItemsOnList)
        {
            _dbContext.Entry(onListMap).State = EntityState.Added;
        }

        cancellationToken.ThrowIfCancellationRequested();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Entities.ShoppingList?> FindEntityByIdAsync(ShoppingListId id)
    {
        return await GetShoppingListQuery()
            .FirstOrDefaultAsync(list => list.Id == id.Value);
    }

    private IQueryable<Entities.ShoppingList> GetShoppingListQuery()
    {
        return _dbContext.ShoppingLists.AsNoTracking()
            .Include(l => l.ItemsOnList);
    }

    #endregion private methods
}