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

namespace ShoppingList.Api.Infrastructure.Adapters
{
    public class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly ShoppingContext dbContext;

        public ItemCategoryRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Domain.Models.ItemCategory>> FindByAsync(string searchInput,
            CancellationToken cancellationToken)
        {
            var itemCategoryEntities = await dbContext.ItemCategories.AsNoTracking()
                .Where(category => category.Name.Contains(searchInput))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return itemCategoryEntities.Select(entity => entity.ToDomain());
        }

        public async Task<Domain.Models.ItemCategory> FindByAsync(ItemCategoryId id,
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

        public async Task<IEnumerable<Domain.Models.ItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids,
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