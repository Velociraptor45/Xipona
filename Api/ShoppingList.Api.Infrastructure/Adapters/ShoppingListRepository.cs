using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Adapters
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ShoppingContext dbContext;
        private readonly IToDomainConverter<Entities.ShoppingList, IShoppingList> toModelConverter;
        private readonly IToEntityConverter<IShoppingList, Entities.ShoppingList> toEntityConverter;

        public ShoppingListRepository(ShoppingContext dbContext,
            IToDomainConverter<Entities.ShoppingList, IShoppingList> toModelConverter,
            IToEntityConverter<IShoppingList, Entities.ShoppingList> toEntityConverter)
        {
            this.dbContext = dbContext;
            this.toModelConverter = toModelConverter;
            this.toEntityConverter = toEntityConverter;
        }

        #region public methods

        public async Task StoreAsync(IShoppingList shoppingList, CancellationToken cancellationToken)
        {
            if (shoppingList == null)
                throw new ArgumentNullException(nameof(shoppingList));

            if (shoppingList.Id.Value <= 0)
            {
                await StoreAsNewListAsync(shoppingList, cancellationToken);
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            var listEntity = await FindEntityByIdAsync(shoppingList.Id);

            cancellationToken.ThrowIfCancellationRequested();

            await StoreModifiedListAsync(listEntity, shoppingList, cancellationToken);
        }

        public async Task<IShoppingList> FindByAsync(ShoppingListId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = await GetShoppingListQuery()
                .FirstOrDefaultAsync(list => list.Id == id.Value);

            cancellationToken.ThrowIfCancellationRequested();

            return toModelConverter.ToDomain(entity);
        }

        public async Task<IEnumerable<IShoppingList>> FindByAsync(Domain.StoreItems.Models.ItemId storeItemId,
            CancellationToken cancellationToken)
        {
            if (storeItemId is null)
                throw new ArgumentNullException(nameof(storeItemId));

            List<Entities.ShoppingList> entities = await GetShoppingListQuery()
                .Where(l => l.ItemsOnList.FirstOrDefault(i => i.ItemId == storeItemId.Value) != null)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return toModelConverter.ToDomain(entities);
        }

        public async Task<IEnumerable<IShoppingList>> FindActiveByAsync(Domain.StoreItems.Models.ItemId storeItemId,
            CancellationToken cancellationToken)
        {
            if (storeItemId is null)
                throw new ArgumentNullException(nameof(storeItemId));

            List<Entities.ShoppingList> entities = await GetShoppingListQuery()
                .Where(l => l.ItemsOnList.FirstOrDefault(i => i.ItemId == storeItemId.Value) != null
                    && l.CompletionDate == null)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return toModelConverter.ToDomain(entities);
        }

        public async Task<IShoppingList> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken)
        {
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var entity = await GetShoppingListQuery()
                .FirstOrDefaultAsync(list => list.CompletionDate == null
                    && list.StoreId == storeId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
                return null;

            return toModelConverter.ToDomain(entity);
        }

        #endregion public methods

        #region private methods

        private async Task StoreModifiedListAsync(Entities.ShoppingList existingShoppingListEntity,
            IShoppingList shoppingList, CancellationToken cancellationToken)
        {
            var shoppingListEntityToStore = toEntityConverter.ToEntity(shoppingList);
            var onListMappings = existingShoppingListEntity.ItemsOnList.ToDictionary(map => map.ItemId);

            dbContext.Entry(shoppingListEntityToStore).State = EntityState.Modified;
            foreach (var map in shoppingListEntityToStore.ItemsOnList)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (onListMappings.TryGetValue(map.ItemId, out var existingMapping))
                {
                    // mapping was modified
                    map.Id = existingMapping.Id;
                    dbContext.Entry(map).State = EntityState.Modified;
                    onListMappings.Remove(map.ItemId);
                }
                else
                {
                    // mapping was added
                    dbContext.Entry(map).State = EntityState.Added;
                }
            }

            // mapping was deleted
            foreach (var map in onListMappings.Values)
            {
                dbContext.Entry(map).State = EntityState.Deleted;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();
        }

        private async Task StoreAsNewListAsync(IShoppingList shoppingList, CancellationToken cancellationToken)
        {
            var entity = toEntityConverter.ToEntity(shoppingList);

            dbContext.Entry(entity).State = EntityState.Added;
            foreach (var onListMap in entity.ItemsOnList)
            {
                dbContext.Entry(onListMap).State = EntityState.Added;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();
        }

        private async Task<Entities.ShoppingList> FindEntityByIdAsync(ShoppingListId id)
        {
            return await GetShoppingListQuery()
                .FirstOrDefaultAsync(list => list.Id == id.Value);
        }

        private IQueryable<Entities.ShoppingList> GetShoppingListQuery()
        {
            return dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.ItemsOnList);
        }

        #endregion private methods
    }
}