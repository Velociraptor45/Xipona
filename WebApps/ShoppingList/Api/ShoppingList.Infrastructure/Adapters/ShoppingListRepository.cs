using Microsoft.EntityFrameworkCore;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Ports;
using ShoppingList.Infrastructure.Converters;
using ShoppingList.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Models = ShoppingList.Domain.Models;

namespace ShoppingList.Infrastructure.Adapters
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ShoppingContext dbContext;

        public ShoppingListRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async void StoreAsync(Models.ShoppingList shoppingList)
        {
        }

        public async Task<Models.ShoppingList> FindActiveByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken)
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

        public async Task<IEnumerable<Models.Store>> FindActiveStoresAsync(CancellationToken cancellationToken)
        {
            var storeEntities = await dbContext.Stores.AsNoTracking()
                .Where(store => !store.Deleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return storeEntities.Select(store => store.ToDomain());
        }

        public async Task<IEnumerable<Models.ItemCategory>> FindItemCategoriesByAsync(string searchInput,
            CancellationToken cancellationToken)
        {
            var itemCategoryEntities = await dbContext.ItemCategories.AsNoTracking()
                .Where(category => category.Name.Contains(searchInput))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return itemCategoryEntities.Select(entity => entity.ToDomain());
        }

        public async Task<IEnumerable<Models.Manufacturer>> FindManufacturersByAsync(string searchInput,
            CancellationToken cancellationToken)
        {
            var manufacturerEntities = await dbContext.Manufacturers.AsNoTracking()
                .Where(category => category.Name.Contains(searchInput))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return manufacturerEntities.Select(entity => entity.ToDomain());
        }
    }
}