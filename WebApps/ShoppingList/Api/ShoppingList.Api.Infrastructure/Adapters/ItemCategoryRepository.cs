using Microsoft.EntityFrameworkCore;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Infrastructure.Converters;
using ShoppingList.Api.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Infrastructure.Adapters
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

        public async Task<IEnumerable<Models.ItemCategory>> FindByAsync(bool includeDeleted,
            CancellationToken cancellationToken)
        {
            var results = await dbContext.ItemCategories.AsNoTracking()
                .Where(m => !m.Deleted || includeDeleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return results.Select(m => m.ToDomain());
        }
    }
}