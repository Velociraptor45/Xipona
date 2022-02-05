using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Adapters;

public class ItemTypeReadRepository : IItemTypeReadRepository
{
    private readonly ItemContext _dbContext;

    public ItemTypeReadRepository(ItemContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<(ItemId, ItemTypeId)>> FindActiveByAsync(string name, StoreId storeId,
        CancellationToken cancellationToken)
    {
        var entries = await _dbContext.ItemTypes.AsNoTracking()
            .Include(t => t.Item)
            .Where(type =>
                !type.Item.Deleted
                && type.Name.Contains(name)
                && type.AvailableAt.Any(av => av.StoreId == storeId.Value))
            .Select(type => new { type.ItemId, type.Id })
            .ToListAsync(cancellationToken);

        return entries
            .Select(type => (new ItemId(type.ItemId), new ItemTypeId(type.Id)));
    }
}