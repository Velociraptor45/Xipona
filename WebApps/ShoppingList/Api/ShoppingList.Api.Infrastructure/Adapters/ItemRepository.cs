using Microsoft.EntityFrameworkCore;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Infrastructure.Converters;
using ShoppingList.Api.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Infrastructure.Adapters
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
                .FirstOrDefaultAsync(item => item.Id == storeItemId.Value);

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
                .FirstOrDefaultAsync(item => item.Id == storeItemId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            if (itemEntity == null)
                throw new ItemNotFoundException($"Item id {storeItemId.Value} not found.");

            var storeMap = itemEntity.AvailableAt.FirstOrDefault(map => map.StoreId == storeId.Value);
            if (storeMap == null)
                throw new ItemAtStoreNotAvailableException(
                    $"Item {itemEntity.Id} not available at store {storeId.Value}");

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
                    itemCategoryIdLists.Contains(item.ItemCategoryId)
                    && manufacturerIdLists.Contains(item.ManufacturerId))
                .ToListAsync();

            // filtering by store
            var filteredResultByStore = result
                .Where(item => storeIdLists.Intersect(item.AvailableAt.Select(av => av.StoreId)).Any());

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

        public async Task<bool> IsValidIdAsync(StoreItemId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = await dbContext.Items.AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id.Value);

            cancellationToken.ThrowIfCancellationRequested();

            return entity != null;
        }

        public async Task<StoreItem> StoreAsync(StoreItem storeItem, CancellationToken cancellationToken)
        {
            if (storeItem == null)
                throw new ArgumentNullException(nameof(storeItem));

            cancellationToken.ThrowIfCancellationRequested();

            if (storeItem.Id.Value <= 0)
            {
                return await AddNewAsync(storeItem);
            }

            return await StoreExistingAsync(storeItem);
        }

        #endregion public methods

        #region private methods

        private async Task<StoreItem> StoreExistingAsync(StoreItem storeItem)
        {
            var entity = storeItem.ToEntity();
            List<AvailableAt> availabilities = storeItem.Availabilities
                .Select(av => av.ToEntity(storeItem.Id))
                .ToList();

            var existingAvailabilities = await dbContext.AvailableAts.AsNoTracking()
                .Where(map => map.ItemId == storeItem.Id.Value)
                .ToDictionaryAsync(av => av.StoreId);

            foreach (var availability in availabilities)
            {
                if (existingAvailabilities.TryGetValue(availability.StoreId, out var existingAvailability))
                {
                    availability.Id = existingAvailability.Id;
                    dbContext.Entry(availability).State = EntityState.Modified;
                    existingAvailabilities.Remove(availability.ItemId);
                }
                else
                {
                    dbContext.Entry(availability).State = EntityState.Added;
                }
            }

            foreach (var existingAvailability in existingAvailabilities.Values)
            {
                dbContext.Entry(existingAvailability).State = EntityState.Deleted;
            }

            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return storeItem;
        }

        private async Task<StoreItem> AddNewAsync(StoreItem storeItem)
        {
            Item entity = storeItem.ToEntity();
            dbContext.Entry(entity).State = EntityState.Added;
            await dbContext.SaveChangesAsync();

            var id = new StoreItemId(entity.Id);
            List<AvailableAt> availabilityMap = storeItem.Availabilities
                .Select(av => av.ToEntity(id))
                .ToList();

            foreach (var availability in availabilityMap)
            {
                dbContext.Entry(availability).State = EntityState.Added;
            }

            await dbContext.SaveChangesAsync();

            entity = await dbContext.Items.AsNoTracking()
                .Include(item => item.ItemCategory)
                .Include(item => item.Manufacturer)
                .Include(item => item.AvailableAt)
                .ThenInclude(map => map.Store)
                .FirstAsync(item => item.Id == entity.Id);

            return entity.ToStoreItemDomain();
        }

        #endregion private methods
    }
}