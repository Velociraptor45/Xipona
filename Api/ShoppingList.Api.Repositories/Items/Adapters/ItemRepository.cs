using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using Item = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item;

namespace ProjectHermes.ShoppingList.Api.Repositories.Items.Adapters;

public class ItemRepository : IItemRepository
{
    private readonly ItemContext _dbContext;
    private readonly IToDomainConverter<Item, IItem> _toModelConverter;
    private readonly IToEntityConverter<IItem, Item> _toEntityConverter;
    private readonly Func<CancellationToken, IDomainEventDispatcher> _domainEventDispatcherDelegate;

    public ItemRepository(ItemContext dbContext, IToDomainConverter<Item, IItem> toModelConverter,
        IToEntityConverter<IItem, Item> toEntityConverter,
        Func<CancellationToken, IDomainEventDispatcher> domainEventDispatcherDelegate)
    {
        _dbContext = dbContext;
        _toModelConverter = toModelConverter;
        _toEntityConverter = toEntityConverter;
        _domainEventDispatcherDelegate = domainEventDispatcherDelegate;
    }

    #region public methods

    public async Task<IItem?> FindByAsync(ItemId itemId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var itemEntity = await GetItemQuery()
            .FirstOrDefaultAsync(item => item.Id == itemId, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (itemEntity == null)
            return null;

        return _toModelConverter.ToDomain(itemEntity);
    }

    public async Task<IItem?> FindByAsync(TemporaryItemId temporaryItemId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var itemEntity = await GetItemQuery()
            .FirstOrDefaultAsync(item => item.CreatedFrom.HasValue && item.CreatedFrom == temporaryItemId,
                cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (itemEntity == null)
            return null;

        return _toModelConverter.ToDomain(itemEntity);
    }

    public async Task<IEnumerable<IItem>> FindByAsync(StoreId storeId, CancellationToken cancellationToken)
    {
        var entities = await GetItemQuery()
            .Where(item => item.AvailableAt.FirstOrDefault(av => av.StoreId == storeId.Value) != null)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindByAsync(IEnumerable<ItemId> itemIds, CancellationToken cancellationToken)
    {
        var idList = itemIds.Select(id => id.Value).ToList();

        var entities = await GetItemQuery()
            .Where(item => idList.Contains(item.Id))
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindByAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken)
    {
        var entities = await GetItemQuery()
            .Where(i => i.ManufacturerId == manufacturerId.Value)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindPermanentByAsync(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var storeIdLists = storeIds.Select(id => id.Value).ToList();
        var itemCategoryIdLists = itemCategoriesIds.Select(id => id.Value).ToList();
        var manufacturerIdLists = manufacturerIds.Select(id => id.Value).ToList();

        var result = await GetItemQuery()
            .Where(item =>
                !item.IsTemporary
                && itemCategoryIdLists.Contains(item.ItemCategoryId!.Value)
                && ((!item.ManufacturerId.HasValue && !manufacturerIdLists.Any())
                    || manufacturerIdLists.Contains(item.ManufacturerId!.Value)))
            .ToListAsync(cancellationToken);

        // filtering by store
        var filteredResultByStore = result
            .Where(item => (!item.AvailableAt.Any() && !storeIdLists.Any())
                           || storeIdLists.Intersect(item.AvailableAt.Select(av => av.StoreId)).Any())
            .ToList();

        cancellationToken.ThrowIfCancellationRequested();

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

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(string searchInput, CancellationToken cancellationToken)
    {
        var entities = await GetItemQuery()
            .Where(item =>
                !item.Deleted
                && !item.IsTemporary
                && item.Name.Contains(searchInput))
            .ToListAsync(cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(ItemCategoryId itemCategoryId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entities = await GetItemQuery()
            .Where(item => item.ItemCategoryId.HasValue
                           && item.ItemCategoryId == itemCategoryId
                           && !item.Deleted)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IItem?> FindActiveByAsync(ItemId itemId, ItemTypeId? itemTypeId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = GetItemQuery()
            .Where(item => item.ItemCategoryId.HasValue
                           && item.Id == itemId
                           && !item.Deleted);

        if (itemTypeId is null)
            query = query.Where(i => !i.ItemTypes.Any());
        else
            query = query.Where(i => i.ItemTypes.Any(t => t.Id == itemTypeId.Value));

        var entity = await query.FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
            return null;

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(SectionId sectionId, CancellationToken cancellationToken)
    {
        var items = await GetItemQuery()
            .Where(item => !item.Deleted
                           && (item.AvailableAt.Any(av => av.DefaultSectionId == sectionId.Value)
                               || item.ItemTypes.Any(t =>
                                   t.AvailableAt.Any(av => av.DefaultSectionId == sectionId.Value))))
            .ToArrayAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(items);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(IEnumerable<ItemId> itemIds,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var rawItemIds = itemIds.Select(id => id.Value).ToArray();

        var items = await GetItemQuery()
            .Where(item => item.ItemCategoryId.HasValue
                           && rawItemIds.Contains(item.Id)
                           && !item.Deleted)
            .ToArrayAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(items);
    }

    public async Task<IItem> StoreAsync(IItem item, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var existingEntity = await FindTrackedEntityBy(item.Id);

        Guid entityIdToLoad;
        if (existingEntity == null)
        {
            var newEntity = _toEntityConverter.ToEntity(item);
            _dbContext.Add(newEntity);

            await _dbContext.SaveChangesAsync(cancellationToken);
            entityIdToLoad = newEntity.Id;
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
            entityIdToLoad = updatedEntity.Id;
        }

        var dispatcher = _domainEventDispatcherDelegate(cancellationToken);
        await ((AggregateRoot)item).DispatchDomainEvents(dispatcher);

        var e = GetItemQuery().First(i => i.Id == entityIdToLoad);
        return _toModelConverter.ToDomain(e);
    }

    #endregion public methods

    #region private methods

    private IQueryable<Item> GetItemQuery()
    {
        return _dbContext.Items.AsNoTracking()
            .Include(item => item.AvailableAt)
            .Include(item => item.ItemTypes)
            .ThenInclude(itemType => itemType.AvailableAt)
            .Include(item => item.ItemTypes);
    }

    private async Task<Item?> FindTrackedEntityBy(ItemId id)
    {
        return await _dbContext.Items
            .Include(item => item.AvailableAt)
            .Include(item => item.ItemTypes)
            .ThenInclude(itemType => itemType.AvailableAt)
            .FirstOrDefaultAsync(i => i.Id == id);
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