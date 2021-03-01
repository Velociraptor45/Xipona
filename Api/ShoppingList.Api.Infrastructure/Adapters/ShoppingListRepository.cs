using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models;
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
        private readonly IToDomainConverter<Entities.ShoppingList, IShoppingList> shoppingListConverter;

        public ShoppingListRepository(ShoppingContext dbContext,
            IToDomainConverter<Entities.ShoppingList, IShoppingList> shoppingListConverter)
        {
            this.dbContext = dbContext;
            this.shoppingListConverter = shoppingListConverter;
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
            if (listEntity == null)
            {
                throw new DomainException(new ShoppingListNotFoundReason(shoppingList.Id));
            }

            cancellationToken.ThrowIfCancellationRequested();

            await StoreModifiedListAsync(listEntity, shoppingList, cancellationToken);
        }

        public async Task<IShoppingList> FindByAsync(ShoppingListId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = await GetShoppingListQuery()
                .FirstOrDefaultAsync(list => list.Id == id.Value);

            if (entity == null) //todo throw in command handler
                throw new DomainException(new ShoppingListNotFoundReason(id));

            cancellationToken.ThrowIfCancellationRequested();

            return shoppingListConverter.ToDomain(entity);
        }

        public async Task<IEnumerable<IShoppingList>> FindByAsync(StoreItemId storeItemId,
            CancellationToken cancellationToken)
        {
            if (storeItemId is null)
                throw new ArgumentNullException(nameof(storeItemId));

            List<Entities.ShoppingList> entities = await GetShoppingListQuery()
                .Where(l => l.ItemsOnList.FirstOrDefault(i => i.ItemId == storeItemId.Actual.Value) != null)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return shoppingListConverter.ToDomain(entities);
        }

        public async Task<IEnumerable<IShoppingList>> FindActiveByAsync(StoreItemId storeItemId,
            CancellationToken cancellationToken)
        {
            if (storeItemId is null)
                throw new ArgumentNullException(nameof(storeItemId));

            List<Entities.ShoppingList> entities = await GetShoppingListQuery()
                .Where(l => l.ItemsOnList.FirstOrDefault(i => i.ItemId == storeItemId.Actual.Value) != null
                    && l.CompletionDate == null)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return shoppingListConverter.ToDomain(entities);
        }

        public async Task<IShoppingList> FindActiveByAsync(ShoppingListStoreId storeId, CancellationToken cancellationToken)
        {
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var entity = await GetShoppingListQuery()
                .FirstOrDefaultAsync(list => list.CompletionDate == null
                    && list.StoreId == storeId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
                return null;

            return shoppingListConverter.ToDomain(entity);
        }

        #endregion public methods

        #region private methods

        private async Task StoreModifiedListAsync(Entities.ShoppingList existingShoppingListEntity,
            IShoppingList shoppingList, CancellationToken cancellationToken)
        {
            var shoppingListEntityToStore = shoppingList.ToEntity();
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
            var entity = shoppingList.ToEntity();

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
            return dbContext.ShoppingLists
                .Include(l => l.Store)
                .ThenInclude(s => s.DefaultSection)
                
                .Include(l => l.Store)
                .ThenInclude(s => s.Sections)
                .ThenInclude(s => s.ActualItemsSections)
                .ThenInclude(s => s.Item)
                .ThenInclude(i => i.AvailableAt)
                
                .Include(l => l.Store)
                .ThenInclude(s => s.Sections)
                .ThenInclude(s => s.ActualItemsSections)
                .ThenInclude(s => s.ShoppingList)

                .Include(l => l.Store)
                .ThenInclude(s => s.Sections)
                .ThenInclude(s => s.DefaultItemsInSection)
                
                .Include(l => l.Store)
                .ThenInclude(s => s.Sections)
                .ThenInclude(s => s.Store)
                
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt)
                
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Section);
        }

        #endregion private methods
    }
}