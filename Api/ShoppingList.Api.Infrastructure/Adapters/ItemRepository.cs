using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
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
        private readonly IToDomainConverter<Item, IStoreItem> toModelConverter;
        private readonly IToEntityConverter<IStoreItem, Item> toEntityConverter;

        public ItemRepository(ShoppingContext dbContext, IToDomainConverter<Item, IStoreItem> toModelConverter,
            IToEntityConverter<IStoreItem, Item> toEntityConverter)
        {
            this.dbContext = dbContext;
            this.toModelConverter = toModelConverter;
            this.toEntityConverter = toEntityConverter;
        }

        #region public methods

        public async Task<IStoreItem> FindByAsync(ItemId storeItemId, CancellationToken cancellationToken)
        {
            if (storeItemId is null)
                throw new ArgumentNullException(nameof(storeItemId));

            cancellationToken.ThrowIfCancellationRequested();

            var itemEntity = await GetItemQuery()
                .FirstOrDefaultAsync(item => item.Id == storeItemId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            itemEntity.Predecessor = await LoadPredecessorsAsync(itemEntity);

            return toModelConverter.ToDomain(itemEntity);
        }

        public async Task<IStoreItem> FindByAsync(TemporaryItemId temporaryItemId, CancellationToken cancellationToken)
        {
            if (temporaryItemId is null)
                throw new ArgumentNullException(nameof(temporaryItemId));

            cancellationToken.ThrowIfCancellationRequested();

            var itemEntity = await GetItemQuery()
                .FirstOrDefaultAsync(item => item.CreatedFrom.HasValue && item.CreatedFrom == temporaryItemId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            itemEntity.Predecessor = await LoadPredecessorsAsync(itemEntity);

            return toModelConverter.ToDomain(itemEntity);
        }

        public async Task<IEnumerable<IStoreItem>> FindByAsync(StoreId storeId, CancellationToken cancellationToken)
        {
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var entities = await GetItemQuery()
                .Where(item => item.AvailableAt.FirstOrDefault(av => av.StoreId == storeId.Value) != null)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var item in entities)
            {
                item.Predecessor = await LoadPredecessorsAsync(item);
            }

            return toModelConverter.ToDomain(entities);
        }

        public async Task<IEnumerable<IStoreItem>> FindByAsync(IEnumerable<ItemId> itemIds, CancellationToken cancellationToken)
        {
            if (itemIds == null)
                throw new ArgumentNullException(nameof(itemIds));

            var idList = itemIds.Select(id => id.Value).ToList();

            var entities = await GetItemQuery()
                .Where(item => idList.Contains(item.Id))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var item in entities)
            {
                item.Predecessor = await LoadPredecessorsAsync(item);
            }

            return toModelConverter.ToDomain(entities);
        }

        public async Task<IEnumerable<IStoreItem>> FindPermanentByAsync(IEnumerable<StoreId> storeIds,
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

            var result = await GetItemQuery()
                .Where(item =>
                    !item.IsTemporary
                    && itemCategoryIdLists.Contains(item.ItemCategoryId.Value)
                    && ((!item.ManufacturerId.HasValue && !manufacturerIdLists.Any())
                        || manufacturerIdLists.Contains(item.ManufacturerId.Value)))
                .ToListAsync();

            // filtering by store
            var filteredResultByStore = result
                .Where(item => (!item.AvailableAt.Any() && !storeIdLists.Any())
                    || storeIdLists.Intersect(item.AvailableAt.Select(av => av.StoreId)).Any())
                .ToList();

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var item in filteredResultByStore)
            {
                item.Predecessor = await LoadPredecessorsAsync(item);
            }

            return toModelConverter.ToDomain(filteredResultByStore);
        }

        public async Task<IEnumerable<IStoreItem>> FindActiveByAsync(string searchInput, StoreId storeId,
            CancellationToken cancellationToken)
        {
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var entities = await GetItemQuery()
                .Where(item => item.Name.Contains(searchInput)
                    && !item.Deleted
                    && !item.IsTemporary
                    && item.AvailableAt.FirstOrDefault(map => map.StoreId == storeId.Value) != null)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var item in entities)
            {
                item.Predecessor = await LoadPredecessorsAsync(item);
            }

            return toModelConverter.ToDomain(entities);
        }

        public async Task<IEnumerable<IStoreItem>> FindActiveByAsync(ItemCategoryId itemCategoryId,
            CancellationToken cancellationToken)
        {
            if (itemCategoryId is null)
            {
                throw new ArgumentNullException(nameof(itemCategoryId));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var entities = await GetItemQuery()
                .Where(item => item.ItemCategoryId.HasValue
                    && item.ItemCategoryId == itemCategoryId.Value
                    && !item.Deleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return toModelConverter.ToDomain(entities);
        }

        public async Task<IStoreItem> StoreAsync(IStoreItem storeItem, CancellationToken cancellationToken)
        {
            if (storeItem == null)
                throw new ArgumentNullException(nameof(storeItem));

            cancellationToken.ThrowIfCancellationRequested();

            var existingEntity = await FindTrackedEntityBy(storeItem.Id);

            if (existingEntity == null)
            {
                var newEntity = toEntityConverter.ToEntity(storeItem);
                dbContext.Add(newEntity);

                if (newEntity.Manufacturer != null)
                    dbContext.Entry(newEntity.Manufacturer).State = EntityState.Unchanged;
                if (newEntity.ItemCategory != null)
                    dbContext.Entry(newEntity.ItemCategory).State = EntityState.Unchanged;

                await dbContext.SaveChangesAsync();

                var e = GetItemQuery().First(i => i.Id == newEntity.Id);
                return toModelConverter.ToDomain(e);
            }
            else
            {
                var updatedEntity = toEntityConverter.ToEntity(storeItem);
                dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                dbContext.Entry(existingEntity).State = EntityState.Modified;

                UpdateOrAddAvailabilities(existingEntity, updatedEntity);
                DeleteAvailabilities(existingEntity, updatedEntity);

                await dbContext.SaveChangesAsync();

                var e = GetItemQuery().First(i => i.Id == updatedEntity.Id);
                return toModelConverter.ToDomain(e);
            }
        }

        #endregion public methods

        #region private methods

        private IQueryable<Item> GetItemQuery()
        {
            return dbContext.Items.AsNoTracking()
                .Include(item => item.ItemCategory)
                .Include(item => item.Manufacturer)
                .Include(item => item.AvailableAt)
                .ThenInclude(map => map.Store)
                .ThenInclude(store => store.Sections);
        }

        private async Task<Item> LoadPredecessorsAsync(Item item)
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

        private async Task<Item> FindTrackedEntityBy(ItemId id)
        {
            return await dbContext.Items
                .Include(item => item.AvailableAt)
                .FirstOrDefaultAsync(i => i.Id == id.Value);
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