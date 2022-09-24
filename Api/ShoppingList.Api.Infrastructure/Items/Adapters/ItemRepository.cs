using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;
using Item = ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities.Item;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Items.Adapters;

public class ItemRepository : IItemRepository
{
    private readonly ItemContext _dbContext;
    private readonly IToDomainConverter<Item, IItem> _toModelConverter;
    private readonly IToEntityConverter<IItem, Item> _toEntityConverter;

    public ItemRepository(ItemContext dbContext, IToDomainConverter<Item, IItem> toModelConverter,
        IToEntityConverter<IItem, Item> toEntityConverter)
    {
        _dbContext = dbContext;
        _toModelConverter = toModelConverter;
        _toEntityConverter = toEntityConverter;
    }

    #region public methods

    public async Task<IItem?> FindByAsync(ItemId itemId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var itemEntity = await GetItemQuery()
            .FirstOrDefaultAsync(item => item.Id == itemId.Value, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (itemEntity == null)
            return null;

        itemEntity.Predecessor = await LoadPredecessorsAsync(itemEntity);

        return _toModelConverter.ToDomain(itemEntity);
    }

    public async Task<IItem?> FindByAsync(TemporaryItemId temporaryItemId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var itemEntity = await GetItemQuery()
            .FirstOrDefaultAsync(item => item.CreatedFrom.HasValue && item.CreatedFrom == temporaryItemId.Value,
                cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (itemEntity == null)
            return null;

        itemEntity.Predecessor = await LoadPredecessorsAsync(itemEntity);

        return _toModelConverter.ToDomain(itemEntity);
    }

    public async Task<IEnumerable<IItem>> FindByAsync(StoreId storeId, CancellationToken cancellationToken)
    {
        var entities = await GetItemQuery()
            .Where(item => item.AvailableAt.FirstOrDefault(av => av.StoreId == storeId.Value) != null)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var item in entities)
        {
            item.Predecessor = await LoadPredecessorsAsync(item);
        }

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindByAsync(IEnumerable<ItemId> itemIds, CancellationToken cancellationToken)
    {
        if (itemIds == null)
            throw new ArgumentNullException(nameof(itemIds));

        var idList = itemIds.Select(id => id.Value).ToList();

        var entities = await GetItemQuery()
            .Where(item => idList.Contains(item.Id))
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var item in entities)
        {
            item.Predecessor = await LoadPredecessorsAsync(item);
        }

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindByAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken)
    {
        var entities = await GetItemQuery()
            .Where(i => i.ManufacturerId == manufacturerId.Value)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var item in entities)
        {
            item.Predecessor = await LoadPredecessorsAsync(item);
        }

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindPermanentByAsync(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds,
        CancellationToken cancellationToken)
    {
        if (storeIds is null)
            throw new ArgumentNullException(nameof(storeIds));
        if (itemCategoriesIds is null)
            throw new ArgumentNullException(nameof(itemCategoriesIds));
        if (manufacturerIds is null)
            throw new ArgumentNullException(nameof(manufacturerIds));

        cancellationToken.ThrowIfCancellationRequested();

        var storeIdLists = storeIds.Select(id => id.Value).ToList();
        var itemCategoryIdLists = itemCategoriesIds.Select(id => id.Value).ToList();
        var manufacturerIdLists = manufacturerIds.Select(id => id.Value).ToList();

        var result = await GetItemQuery()
            .Where(item =>
                !item.IsTemporary
                && itemCategoryIdLists.Contains(item.ItemCategoryId!.Value)
                && (!item.ManufacturerId.HasValue && !manufacturerIdLists.Any()
                    || manufacturerIdLists.Contains(item.ManufacturerId!.Value)))
            .ToListAsync(cancellationToken);

        // filtering by store
        var filteredResultByStore = result
            .Where(item => !item.AvailableAt.Any() && !storeIdLists.Any()
                           || storeIdLists.Intersect(item.AvailableAt.Select(av => av.StoreId)).Any())
            .ToList();

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var item in filteredResultByStore)
        {
            item.Predecessor = await LoadPredecessorsAsync(item);
        }

        return _toModelConverter.ToDomain(filteredResultByStore);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(string searchInput, StoreId storeId,
        CancellationToken cancellationToken)
    {
        var entities = await GetItemQuery()
            .Where(item =>
                !item.Deleted
                && !item.IsTemporary
                && item.Name.Contains(searchInput)
                && (item.AvailableAt.Any(map => map.StoreId == storeId.Value)
                    || item.ItemTypes.Any(t => t.AvailableAt.Any(av => av.StoreId == storeId.Value))))
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var item in entities)
        {
            item.Predecessor = await LoadPredecessorsAsync(item);
        }

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(string searchInput, ItemCategoryId? itemCategoryId,
        CancellationToken cancellationToken)
    {
        var query = GetItemQuery()
            .Where(item =>
                !item.Deleted
                && !item.IsTemporary
                && item.Name.Contains(searchInput));

        if (itemCategoryId.HasValue)
            query = query.Where(i => i.ItemCategoryId == itemCategoryId.Value.Value);

        var entities = await query.ToListAsync(cancellationToken);

        foreach (var item in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();
            item.Predecessor = await LoadPredecessorsAsync(item);
        }

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(ItemCategoryId itemCategoryId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entities = await GetItemQuery()
            .Where(item => item.ItemCategoryId.HasValue
                           && item.ItemCategoryId == itemCategoryId.Value
                           && !item.Deleted)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var item in entities)
        {
            item.Predecessor = await LoadPredecessorsAsync(item);
        }

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IItem?> FindActiveByAsync(ItemId itemId, ItemTypeId? itemTypeId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = GetItemQuery()
            .Where(item => item.ItemCategoryId.HasValue
                           && item.Id == itemId.Value
                           && !item.Deleted);

        if (itemTypeId is null)
            query = query.Where(i => !i.ItemTypes.Any());
        else
            query = query.Where(i => i.ItemTypes.Any(t => t.Id == itemTypeId.Value.Value));

        var entity = await query.FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
            return null;

        cancellationToken.ThrowIfCancellationRequested();

        entity.Predecessor = await LoadPredecessorsAsync(entity);

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IItem> StoreAsync(IItem item, CancellationToken cancellationToken)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        cancellationToken.ThrowIfCancellationRequested();

        var existingEntity = await FindTrackedEntityBy(item.Id);

        if (existingEntity == null)
        {
            var newEntity = _toEntityConverter.ToEntity(item);
            _dbContext.Add(newEntity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var e = GetItemQuery().First(i => i.Id == newEntity.Id);
            e.Predecessor = await LoadPredecessorsAsync(e);
            return _toModelConverter.ToDomain(e);
        }
        else
        {
            var updatedEntity = _toEntityConverter.ToEntity(item);
            updatedEntity.Id = existingEntity.Id;
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            _dbContext.Entry(existingEntity).State = EntityState.Modified;

            UpdateOrAddAvailabilities(existingEntity, updatedEntity);
            DeleteAvailabilities(existingEntity, updatedEntity);
            UpdateOrAddItemTypes(existingEntity, updatedEntity);
            DeleteItemTypes(existingEntity, updatedEntity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var e = GetItemQuery().First(i => i.Id == updatedEntity.Id);
            e.Predecessor = await LoadPredecessorsAsync(e);
            return _toModelConverter.ToDomain(e);
        }
    }

    #endregion public methods

    #region private methods

    private IQueryable<Item> GetItemQuery()
    {
        return _dbContext.Items.AsNoTracking()
            .Include(item => item.AvailableAt)
            .Include(item => item.ItemTypes)
            .ThenInclude(itemType => itemType.AvailableAt)
            .Include(item => item.ItemTypes)
            .ThenInclude(itemType => itemType.Predecessor);
    }

    private async Task<Item?> LoadPredecessorsAsync(Item item)
    {
        if (item.PredecessorId == null)
            return null;

        var predecessor = await GetItemQuery()
            .SingleOrDefaultAsync(i => i.Id == item.PredecessorId.Value);
        if (predecessor == null)
            return null;

        predecessor.Predecessor = await LoadPredecessorsAsync(predecessor);
        return predecessor;
    }

    private async Task<Item?> FindTrackedEntityBy(ItemId id)
    {
        return await _dbContext.Items
            .Include(item => item.AvailableAt)
            .Include(item => item.ItemTypes)
            .ThenInclude(itemType => itemType.AvailableAt)
            .FirstOrDefaultAsync(i => i.Id == id.Value);
    }

    private void UpdateOrAddAvailabilities(Item existing, Item updated)
    {
        foreach (var availability in updated.AvailableAt)
        {
            var existingAvailability = existing.AvailableAt
                .FirstOrDefault(av => MatchesKey(av, availability));

            if (existingAvailability == null)
            {
                existing.AvailableAt.Add(availability);
            }
            else
            {
                _dbContext.Entry(existingAvailability).CurrentValues.SetValues(availability);
            }
        }
    }

    private void DeleteAvailabilities(Item existing, Item updated)
    {
        foreach (var availability in existing.AvailableAt)
        {
            bool hasExistingAvailability = updated.AvailableAt.Any(av => MatchesKey(av, availability));
            if (!hasExistingAvailability)
            {
                _dbContext.Remove(availability);
            }
        }
    }

    private void DeleteItemTypes(Item existing, Item updated)
    {
        foreach (var type in existing.ItemTypes)
        {
            bool hasExistingAvailability = updated.ItemTypes.Any(t => t.Id == type.Id);
            if (!hasExistingAvailability)
            {
                _dbContext.Remove(type);
            }
        }
    }

    private void UpdateOrAddItemTypes(Item existing, Item updated)
    {
        foreach (var updatedType in updated.ItemTypes)
        {
            var existingType = existing.ItemTypes
                .FirstOrDefault(t => t.Id == updatedType.Id);

            if (existingType == null)
            {
                existing.ItemTypes.Add(updatedType);
            }
            else
            {
                _dbContext.Entry(existingType).CurrentValues.SetValues(updatedType);
                UpdateOrAddItemAvailability(existingType, updatedType);
                DeleteItemAvailability(existingType, updatedType);
            }
        }
    }

    private void UpdateOrAddItemAvailability(Entities.ItemType existing, Entities.ItemType updated)
    {
        foreach (var availability in updated.AvailableAt)
        {
            var existingAvailability = existing.AvailableAt.FirstOrDefault(av => MatchesKey(av, availability));

            if (existingAvailability == null)
            {
                existing.AvailableAt.Add(availability);
            }
            else
            {
                _dbContext.Entry(existingAvailability).CurrentValues.SetValues(availability);
            }
        }
    }

    private void DeleteItemAvailability(Entities.ItemType existing, Entities.ItemType updated)
    {
        foreach (var availability in existing.AvailableAt)
        {
            bool hasExistingAvailability = updated.AvailableAt.Any(av => MatchesKey(av, availability));
            if (!hasExistingAvailability)
            {
                _dbContext.Remove(availability);
            }
        }
    }

    private static bool MatchesKey(AvailableAt left, AvailableAt right)
    {
        return left.StoreId == right.StoreId && left.ItemId == right.ItemId;
    }

    private static bool MatchesKey(ItemTypeAvailableAt left, ItemTypeAvailableAt right)
    {
        return left.StoreId == right.StoreId && left.ItemTypeId == right.ItemTypeId;
    }

    #endregion private methods
}