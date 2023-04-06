using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
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
    private readonly ILogger<ShoppingListRepository> _logger;

    public ShoppingListRepository(ShoppingListContext dbContext,
        IToDomainConverter<Entities.ShoppingList, IShoppingList> toDomainConverter,
        IToEntityConverter<IShoppingList, Entities.ShoppingList> toEntityConverter,
        ILogger<ShoppingListRepository> logger)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toEntityConverter = toEntityConverter;
        _logger = logger;
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
            .FirstOrDefaultAsync(list => list.Id == id, cancellationToken);

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
                    && list.StoreId == storeId,
                cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    #endregion public methods

    #region private methods

    private async Task StoreModifiedListAsync(Entities.ShoppingList existingEntity,
        IShoppingList shoppingList, CancellationToken cancellationToken)
    {
        var updatedEntity = _toEntityConverter.ToEntity(shoppingList);
        var onListMappings = existingEntity.ItemsOnList.ToDictionary(map => (map.ItemId, map.ItemTypeId));

        var existingRowVersion = existingEntity.RowVersion;
        _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
        _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
        _dbContext.Entry(existingEntity).State = EntityState.Modified;

        foreach (var map in updatedEntity.ItemsOnList)
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

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex,
                () => $"Saving shopping list {shoppingList.Id.Value} failed due to concurrency violation");
            throw new DomainException(new ModelOutOfDateReason());
        }
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
            .FirstOrDefaultAsync(list => list.Id == id);
    }

    private IQueryable<Entities.ShoppingList> GetShoppingListQuery()
    {
        return _dbContext.ShoppingLists.AsNoTracking()
            .Include(l => l.ItemsOnList);
    }

    #endregion private methods
}