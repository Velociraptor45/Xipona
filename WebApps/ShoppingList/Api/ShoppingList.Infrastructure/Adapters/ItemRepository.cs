using Microsoft.EntityFrameworkCore;
using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Ports;
using ShoppingList.Infrastructure.Converters;
using ShoppingList.Infrastructure.Entities;
using System;
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

        #region public methods

        public async Task<StoreItem> FindByAsync(StoreItemId storeItemId, StoreId storeId, CancellationToken cancellationToken)
        {
            var itemEntity = await dbContext.Items.AsNoTracking()
                .Include(item => item.ItemCategory)
                .Include(item => item.Manufacturer)
                .Include(item => item.AvailableAt)
                .ThenInclude(map => map.Store)
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

        public async Task<StoreItemId> StoreAsync(StoreItem storeItem)
        {
            if (storeItem == null)
                throw new ArgumentNullException(nameof(storeItem));

            if (storeItem.Id.Value <= 0)
            {
                return await AddNewAsync(storeItem);
            }

            return await StoreExistingAsync(storeItem);
        }

        #endregion public methods

        #region private methods

        private async Task<StoreItemId> StoreExistingAsync(StoreItem storeItem)
        {
            var entity = storeItem.ToEntity();
            var itemToStoreMap = storeItem.ToItemMap();
            var existingItemToStoreMap = await dbContext.AvailableAts.AsNoTracking()
                .FirstOrDefaultAsync(map => map.ItemId == itemToStoreMap.ItemId
                && map.StoreId == itemToStoreMap.StoreId);

            dbContext.Entry(entity).State = EntityState.Modified;

            if (existingItemToStoreMap == null)
            {
                dbContext.Entry(itemToStoreMap).State = EntityState.Added;
            }
            else
            {
                dbContext.Entry(itemToStoreMap).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();
            dbContext.Entry(entity).State = EntityState.Detached;
            return storeItem.Id;
        }

        private async Task<StoreItemId> AddNewAsync(StoreItem storeItem)
        {
            Item entity = storeItem.ToEntity();
            AvailableAt itemToStoreMap = storeItem.ToItemMap();

            dbContext.Entry(entity).State = EntityState.Added;
            await dbContext.SaveChangesAsync();
            var id = new StoreItemId(entity.Id);
            itemToStoreMap.ItemId = id.Value;

            dbContext.Entry(itemToStoreMap).State = EntityState.Added;
            await dbContext.SaveChangesAsync();
            dbContext.Entry(entity).State = EntityState.Detached;

            return id;
        }

        #endregion private methods
    }
}