using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ItemType = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.ShoppingList.Api.Repositories.Items.Adapters;

public class ItemTypeReadRepository : IItemTypeReadRepository
{
    private readonly ItemContext _dbContext;
    private readonly IToDomainConverter<ItemType, IItemType> _toDomainConverter;
    private readonly CancellationToken _cancellationToken;

    public ItemTypeReadRepository(ItemContext dbContext, IToDomainConverter<ItemType, IItemType> toDomainConverter,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _cancellationToken = cancellationToken;
    }

    public async Task<IEnumerable<(ItemId, ItemTypeId)>> FindActiveByAsync(string name, StoreId storeId,
        IEnumerable<ItemId> excludedItemIds,
        IEnumerable<ItemTypeId> excludedItemTypeIds, int? limit)
    {
        var excludedRawItemIds = excludedItemIds.Select(id => id.Value).ToList();
        var excludedRawItemTypeIds = excludedItemTypeIds.Select(id => id.Value).ToList();

        var query = _dbContext.ItemTypes.AsNoTracking()
            .Include(t => t.Item)
            .Where(type =>
                !type.Item!.Deleted
                && !type.IsDeleted
                && !excludedRawItemTypeIds.Contains(type.Id)
                && type.Name.Contains(name)
                && type.AvailableAt.Any(av => av.StoreId == storeId)
                && !excludedRawItemIds.Contains(type.Item.Id))
            .Select(type => new { type.ItemId, type.Id });

        if (limit.HasValue)
            query = query.Take(limit.Value);

        var entries = await query.ToListAsync(_cancellationToken);

        return entries.Select(type => (new ItemId(type.ItemId), new ItemTypeId(type.Id)));
    }

    public async Task<IEnumerable<IItemType>> FindByAsync(IEnumerable<ItemTypeId> ids)
    {
        var rawIds = ids.Select(id => id.Value).ToList();

        var entries = await _dbContext.ItemTypes.AsNoTracking()
            .Where(type => rawIds.Contains(type.Id))
            .ToListAsync(_cancellationToken);

        return _toDomainConverter.ToDomain(entries);
    }
}