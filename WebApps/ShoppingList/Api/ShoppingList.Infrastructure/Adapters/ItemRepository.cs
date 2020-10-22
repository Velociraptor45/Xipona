using Microsoft.EntityFrameworkCore;
using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Ports;
using ShoppingList.Infrastructure.Converters;
using ShoppingList.Infrastructure.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Infrastructure.Adapters
{
    public class ItemRepository : IItemRepository
    {
        private readonly ShoppingContext dbContext;

        public ItemRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<StoreItem> FindBy(StoreItemId storeItemId, StoreId storeId, CancellationToken cancellationToken)
        {
            var itemEntity = await dbContext.Items.AsNoTracking()
                .Include(item => item.ItemCategory)
                .Include(item => item.Manufacturer)
                .Include(item => item.AvailableAt)
                .FirstOrDefaultAsync(item => item.Id == storeItemId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            if (itemEntity == null)
                throw new ItemNotFoundException($"Item id {storeItemId.Value} not found.");

            var storeMap = itemEntity.AvailableAt.FirstOrDefault(map => map.StoreId == storeId.Value);
            if (storeMap == null)
                throw new ItemAtStoreNotAvailableException($"Item {itemEntity.Id} not available at store {storeId.Value}");

            cancellationToken.ThrowIfCancellationRequested();

            return itemEntity.ToStoreItemDomain(storeMap.Store, storeMap.Price);
        }
    }
}