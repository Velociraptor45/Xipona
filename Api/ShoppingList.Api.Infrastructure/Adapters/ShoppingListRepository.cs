using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities;
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
        private readonly IShoppingListFactory shoppingListFactory;
        private readonly ISectionFactory sectionFactory;

        public ShoppingListRepository(ShoppingContext dbContext, IShoppingListFactory shoppingListFactory,
            ISectionFactory sectionFactory)
        {
            this.dbContext = dbContext;
            this.shoppingListFactory = shoppingListFactory;
            this.sectionFactory = sectionFactory;
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

            var sections = GetSections(entity);

            return shoppingListFactory.Create(
                new ShoppingListId(entity.Id),
                entity.Store.ToDomain(),
                sections,
                entity.CompletionDate);
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

            return entities.Select(entity =>
            {
                var sections = GetSections(entity);

                return shoppingListFactory.Create(
                    new ShoppingListId(entity.Id),
                    entity.Store.ToDomain(),
                    sections,
                    entity.CompletionDate);
            });
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

            return entities.Select(entity =>
            {
                var sections = GetSections(entity);

                return shoppingListFactory.Create(
                    new ShoppingListId(entity.Id),
                    entity.Store.ToDomain(),
                    sections,
                    entity.CompletionDate);
            });
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

            var sections = GetSections(entity);

            return shoppingListFactory.Create(
                new ShoppingListId(entity.Id),
                entity.Store.ToDomain(),
                sections,
                entity.CompletionDate);
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
            return dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.Store)
                .ThenInclude(s => s.Sections)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt);
        }

        private IEnumerable<IShoppingListSection> GetSections(Entities.ShoppingList shoppingListEntity)
        {
            var itemsLookup = shoppingListEntity.ItemsOnList.ToLookup(i => i.SectionId.Value);
            foreach (var section in shoppingListEntity.Store.Sections)
            {
                var itemsForSection = itemsLookup[section.Id];
                var domainItems = itemsForSection
                    .Select(map => map.Item.ToShoppingListItemDomain(shoppingListEntity.StoreId, shoppingListEntity.Id));
                var domainSection = sectionFactory.Create(new ShoppingListSectionId(section.Id), section.Name, domainItems,
                    section.SortIndex, shoppingListEntity.Store.DefaultSection.Id == section.Id);
                yield return domainSection;
            }
        }

        #endregion private methods
    }
}