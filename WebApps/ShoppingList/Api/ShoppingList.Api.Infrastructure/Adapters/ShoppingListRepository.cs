using Microsoft.EntityFrameworkCore;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Infrastructure.Converters;
using ShoppingList.Api.Infrastructure.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Infrastructure.Adapters
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ShoppingContext dbContext;

        public ShoppingListRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region public methods

        public async Task StoreAsync(Domain.Models.ShoppingList shoppingList, CancellationToken cancellationToken)
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
                throw new ItemNotOnShoppingListException($"Shopping list with ID {shoppingList.Id.Value} not found.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            await StoreModifiedListAsync(listEntity, shoppingList, cancellationToken);
        }

        public async Task<Domain.Models.ShoppingList> FindByAsync(ShoppingListId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var list = await dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.Store)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt)
                .FirstOrDefaultAsync(list => list.Id == id.Value);

            if (list == null)
                throw new ShoppingListNotFoundException($"Shopping list {id.Value} not found");

            cancellationToken.ThrowIfCancellationRequested();

            return list.ToDomain();
        }

        public async Task<Domain.Models.ShoppingList> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken)
        {
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var list = await dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.Store)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt)
                .FirstOrDefaultAsync(list => list.CompletionDate == null
                    && list.StoreId == storeId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            return list?.ToDomain();
        }

        public async Task<bool> ActiveShoppingListExistsForAsync(StoreId storeId, CancellationToken cancellationToken)
        {
            var list = await dbContext.ShoppingLists.AsNoTracking()
                .FirstOrDefaultAsync(list => list.StoreId == storeId.Value
                    && list.CompletionDate == null);

            cancellationToken.ThrowIfCancellationRequested();

            return list != null;
        }

        #endregion public methods

        #region private methods

        private async Task StoreModifiedListAsync(Entities.ShoppingList existingShoppingListEntity,
            Domain.Models.ShoppingList shoppingList, CancellationToken cancellationToken)
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

        private async Task StoreAsNewListAsync(Domain.Models.ShoppingList shoppingList, CancellationToken cancellationToken)
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
            return await dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.Store)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt)
                .FirstOrDefaultAsync(list => list.Id == id.Value);
        }

        #endregion private methods
    }
}