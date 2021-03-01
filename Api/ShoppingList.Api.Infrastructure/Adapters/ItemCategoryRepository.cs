using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Adapters
{
    public class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly ShoppingContext dbContext;
        private readonly IToDomainConverter<Entities.ItemCategory, IItemCategory> itemCategoryConverter;

        public ItemCategoryRepository(ShoppingContext dbContext,
            IToDomainConverter<Entities.ItemCategory, IItemCategory> itemCategoryConverter)
        {
            this.dbContext = dbContext;
            this.itemCategoryConverter = itemCategoryConverter;
        }

        public async Task<IEnumerable<IItemCategory>> FindByAsync(string searchInput,
            CancellationToken cancellationToken)
        {
            var itemCategoryEntities = await dbContext.ItemCategories.AsNoTracking()
                .Where(category => category.Name.Contains(searchInput))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return itemCategoryConverter.ToDomain(itemCategoryEntities);
        }

        public async Task<IItemCategory> FindByAsync(ItemCategoryId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = await dbContext.ItemCategories.AsNoTracking()
                .FirstOrDefaultAsync(category => category.Id == id.Value);

            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
                throw new DomainException(new ItemCategoryNotFoundReason(id));

            return itemCategoryConverter.ToDomain(entity);
        }

        public async Task<IEnumerable<IItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids,
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

            return itemCategoryConverter.ToDomain(entities);
        }

        public async Task<IEnumerable<IItemCategory>> FindActiveByAsync(CancellationToken cancellationToken)
        {
            var entities = await dbContext.ItemCategories.AsNoTracking()
                .Where(m => !m.Deleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return itemCategoryConverter.ToDomain(entities);
        }

        public async Task<IItemCategory> StoreAsync(IItemCategory model,
            CancellationToken cancellationToken)
        {
            var entity = model.ToEntity();

            if (entity.Id <= 0)
            {
                dbContext.Entry(entity).State = EntityState.Added;
            }
            else
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();
            return itemCategoryConverter.ToDomain(entity);
        }
    }
}