using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;

namespace ProjectHermes.ShoppingList.Api.Repositories.Items.Adapters;

public class ItemTypeReadRepository : IItemTypeReadRepository
{
    private readonly ItemContext _dbContext;
    private readonly CancellationToken _cancellationToken;

    public ItemTypeReadRepository(ItemContext dbContext, CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _cancellationToken = cancellationToken;
    }

    public async Task<IEnumerable<(ItemId, ItemTypeId)>> FindActiveByAsync(string name, StoreId storeId,
        IEnumerable<ItemId> excludedItemIds,
        IEnumerable<ItemTypeId> excludedItemTypeIds, int? limit)
    {
        var excludedRawItemTypeIds = excludedItemTypeIds.Select(id => id.Value).ToList();

        var query = _dbContext.ItemTypes.AsNoTracking()
            .Include(t => t.Item)
            .Where(type =>
                !type.Item.Deleted
                && !type.IsDeleted
                && !excludedRawItemTypeIds.Contains(type.Id)
                && type.Name.Contains(name)
                && type.AvailableAt.Any(av => av.StoreId == storeId))
            .Select(type => new { type.ItemId, type.Id });

        if (limit.HasValue)
            query = query.Take(limit.Value);

        var entries = await query.ToListAsync(_cancellationToken);

        return entries.Select(type => (new ItemId(type.ItemId), new ItemTypeId(type.Id)));
    }
}