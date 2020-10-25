using Microsoft.EntityFrameworkCore;
using ShoppingList.Domain.Exceptions;
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
    public class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly ShoppingContext dbContext;

        public ItemCategoryRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Models.ItemCategory>> FindByAsync(string searchInput,
            CancellationToken cancellationToken)
        {
            var itemCategoryEntities = await dbContext.ItemCategories.AsNoTracking()
                .Where(category => category.Name.Contains(searchInput))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return itemCategoryEntities.Select(entity => entity.ToDomain());
        }

        public async Task<Models.ItemCategory> FindByAsync(ItemCategoryId id,
            CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = await dbContext.ItemCategories.AsNoTracking()
                .FirstOrDefaultAsync(category => category.Id == id.Value);

            if (entity == null)
                throw new ItemCategoryNotFoundException($"Item category {id.Value} not found.");

            return entity.ToDomain();
        }

        public async Task<IEnumerable<Models.ItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids,
            CancellationToken cancellationToken)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var idHashSet = ids.Select(id => id.Value).ToHashSet();

            cancellationToken.ThrowIfCancellationRequested();

            var entities = await dbContext.ItemCategories.AsNoTracking()
                .Where(m => idHashSet.Contains(m.Id))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return entities.Select(e => e.ToDomain());
        }
    }
}