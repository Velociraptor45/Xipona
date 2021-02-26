using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Adapters
{
    public class StoreItemSectionReadRepository : IStoreItemSectionReadRepository
    {
        private readonly ShoppingContext dbContext;
        private readonly IStoreItemSectionFactory storeItemSectionFactory;

        public StoreItemSectionReadRepository(ShoppingContext dbContext,
            IStoreItemSectionFactory storeItemSectionFactory)
        {
            this.dbContext = dbContext;
            this.storeItemSectionFactory = storeItemSectionFactory;
        }

        public async Task<IEnumerable<IStoreItemSection>> FindByAsync(IEnumerable<StoreItemSectionId> storeItemSectionIds,
            CancellationToken cancellationToken)
        {
            var sectionIds = storeItemSectionIds.Select(s => s.Value);

            var entities = await dbContext.Sections.AsNoTracking()
                .Where(section => sectionIds.Contains(section.Id))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            var sectionList = new List<IStoreItemSection>();
            foreach (var entity in entities)
            {
                var section = storeItemSectionFactory.Create(
                    new StoreItemSectionId(entity.Id), entity.Name, entity.SortIndex);
                sectionList.Add(section);
            }

            return sectionList;
        }

        public async Task<IStoreItemSection> FindByAsync(StoreItemSectionId storeItemSectionId,
            CancellationToken cancellationToken)
        {
            var entity = await dbContext.Sections.AsNoTracking()
                .FirstOrDefaultAsync(section => section.Id == storeItemSectionId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
                return null;

            return storeItemSectionFactory.Create(new StoreItemSectionId(entity.Id), entity.Name, entity.SortIndex);
        }
    }
}