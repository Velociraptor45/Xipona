using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Adapters
{
    public class ItemTypeReadRepository : IItemTypeReadRepository
    {
        private readonly ItemContext _dbContext;

        public ItemTypeReadRepository(ItemContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ItemTypeId>> FindByAsync(string name, StoreId storeId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.ItemTypes.AsNoTracking()
                .Where(type => type.Name.Contains(name)
                    && type.AvailableAt.Any(av => av.StoreId == storeId.Value))
                .Select(type => new ItemTypeId(type.Id))
                .ToListAsync(cancellationToken);
        }
    }
}