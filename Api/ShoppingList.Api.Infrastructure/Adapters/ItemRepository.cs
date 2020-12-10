using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models;
using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Adapters
{
    public class ItemRepository : IItemRepository
    {
        private readonly ShoppingContext dbContext;

        public ItemRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region public methods

        public async Task<StoreItem> FindByAsync(StoreItemId storeItemId, CancellationToken cancellationToken)
        {
            if (storeItemId is null)
            {
                throw new ArgumentNullException(nameof(storeItemId));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var itemEntity = await dbContext.Items.AsNoTracking()
                .Include(item => item.ItemCategory)
                .Include(item => item.Manufacturer)
                .Include(item => item.AvailableAt)
                .ThenInclude(map => map.Store)
                .FirstOrDefaultAsync(item => storeItemId.IsActualId ?
                    item.Id == storeItemId.Actual.Value :
                    item.CreatedFrom == storeItemId.Offline.Value);

            if (itemEntity == null)
                throw new ItemNotFoundException(storeItemId);

            cancellationToken.ThrowIfCancellationRequested();

            return itemEntity.ToStoreItemDomain();
        }

        public async Task<StoreItem> FindByAsync(StoreItemId storeItemId, StoreId storeId,
            CancellationToken cancellationToken)
        {
            if (storeItemId == null)
                throw new ArgumentNullException(nameof(storeItemId));
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var itemEntity = await dbContext.Items.AsNoTracking()
                .Include(item => item.ItemCategory)
                .Include(item => item.Manufacturer)
                .Include(item => item.AvailableAt)
                .ThenInclude(map => map.Store)
                .FirstOrDefaultAsync(item => storeItemId.IsActualId ?
                    item.Id == storeItemId.Actual.Value :
                    item.CreatedFrom == storeItemId.Offline.Value);

            cancellationToken.ThrowIfCancellationRequested();

            if (itemEntity == null)
                throw new ItemNotFoundException(storeItemId);

            var storeMap = itemEntity.AvailableAt.FirstOrDefault(map => map.StoreId == storeId.Value);
            if (storeMap == null)
                throw new ItemAtStoreNotAvailableException(storeItemId, storeId);

            cancellationToken.ThrowIfCancellationRequested();

            return itemEntity.ToStoreItemDomain();
        }

        public async Task<IEnumerable<StoreItem>> FindByAsync(IEnumerable<StoreId> storeIds,
            IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds,
            CancellationToken cancellationToken)
        {
            if (storeIds is null)
            {
                throw new ArgumentNullException(nameof(storeIds));
            }
            else if (itemCategoriesIds is null)
            {
                throw new ArgumentNullException(nameof(itemCategoriesIds));
            }
            else if (manufacturerIds is null)
            {
                throw new ArgumentNullException(nameof(manufacturerIds));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var storeIdLists = storeIds.Select(id => id.Value).ToList();
            var itemCategoryIdLists = itemCategoriesIds.Select(id => id.Value).ToList();
            var manufacturerIdLists = manufacturerIds.Select(id => id.Value).ToList();

            var result = await dbContext.Items.AsNoTracking()
                .Include(item => item.ItemCategory)
                .Include(item => item.Manufacturer)
                .Include(item => item.AvailableAt)
                .ThenInclude(map => map.Store)
                .Where(item =>
                    !item.IsTemporary
                    && itemCategoryIdLists.Contains(item.ItemCategoryId.Value)
                    && ((!item.ManufacturerId.HasValue && !manufacturerIdLists.Any())
                        || manufacturerIdLists.Contains(item.ManufacturerId.Value)))
                .ToListAsync();

            // filtering by store
            var filteredResultByStore = result
                .Where(item => (!item.AvailableAt.Any() && !storeIdLists.Any())
                    || storeIdLists.Intersect(item.AvailableAt.Select(av => av.StoreId)).Any());

            cancellationToken.ThrowIfCancellationRequested();

            return filteredResultByStore.Select(r => r.ToStoreItemDomain());
        }

        public async Task<IEnumerable<StoreItem>> FindByAsync(string searchInput, StoreId storeId,
            CancellationToken cancellationToken)
        {
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var entities = await dbContext.Items.AsNoTracking()
                .Include(item => item.ItemCategory)
                .Include(item => item.Manufacturer)
                .Include(item => item.AvailableAt)
                .ThenInclude(map => map.Store)
                .Where(item => item.Name.Contains(searchInput)
                    && !item.Deleted
                    && !item.IsTemporary
                    && item.AvailableAt.FirstOrDefault(map => map.StoreId == storeId.Value) != null)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return entities.Select(e => e.ToStoreItemDomain());
        }

        public async Task<IEnumerable<StoreItem>> FindByAsync(StoreId storeId, CancellationToken cancellationToken)
        {
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var entities = await dbContext.Items.AsNoTracking()
                .Include(item => item.ItemCategory)
                .Include(item => item.Manufacturer)
                .Include(item => item.AvailableAt)
                .ThenInclude(map => map.Store)
                .Where(item => item.AvailableAt.FirstOrDefault(av => av.StoreId == storeId.Value) != null)
                .ToListAsync();

            return entities.Select(e => e.ToStoreItemDomain());
        }

        public async Task<StoreItem> StoreAsync(StoreItem storeItem, CancellationToken cancellationToken)
        {
            if (storeItem == null)
                throw new ArgumentNullException(nameof(storeItem));

            cancellationToken.ThrowIfCancellationRequested();

            var existingEntity = await FindTrackedEntityBy(storeItem.Id);

            if (existingEntity == null)
            {
                var newEntity = storeItem.ToEntity();
                dbContext.Add(newEntity);

                if (newEntity.Manufacturer != null)
                    dbContext.Entry(newEntity.Manufacturer).State = EntityState.Unchanged;
                if (newEntity.ItemCategory != null)
                    dbContext.Entry(newEntity.ItemCategory).State = EntityState.Unchanged;

                await dbContext.SaveChangesAsync();
                return newEntity.ToStoreItemDomain();
            }
            else
            {
                var updatedEntity = storeItem.ToEntity();
                dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                dbContext.Entry(existingEntity).State = EntityState.Modified;

                UpdateOrAddAvailabilities(existingEntity, updatedEntity);
                DeleteAvailabilities(existingEntity, updatedEntity);

                await dbContext.SaveChangesAsync();
                return existingEntity.ToStoreItemDomain();
            }
        }

        #endregion public methods

        #region private methods

        private async Task<Item> FindTrackedEntityBy(StoreItemId id)
        {
            return await dbContext.Items
                .Include(item => item.AvailableAt)
                .FirstOrDefaultAsync(i => id.IsActualId ?
                    i.Id == id.Actual.Value :
                    i.CreatedFrom == id.Offline.Value);
        }

        private void UpdateOrAddAvailabilities(Item existing, Item updated)
        {
            foreach (var availability in updated.AvailableAt)
            {
                var exisitingAvailability = existing.AvailableAt
                    .FirstOrDefault(av => av.Id == availability.Id);

                if (exisitingAvailability == null)
                {
                    existing.AvailableAt.Add(availability);
                }
                else
                {
                    dbContext.Entry(exisitingAvailability).CurrentValues.SetValues(availability);
                }
            }
        }

        private void DeleteAvailabilities(Item existing, Item updated)
        {
            foreach (var availability in existing.AvailableAt)
            {
                bool hasExistingAvailability = updated.AvailableAt.Any(av => av.Id == availability.Id);
                if (!hasExistingAvailability)
                {
                    dbContext.Remove(availability);
                }
            }
        }

        #endregion private methods
    }
}