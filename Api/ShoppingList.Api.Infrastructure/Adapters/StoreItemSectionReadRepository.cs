using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
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
        private readonly IToDomainConverter<Section, IStoreItemSection> toDomainConverter;

        public StoreItemSectionReadRepository(ShoppingContext dbContext,
            IToDomainConverter<Section, IStoreItemSection> toDomainConverter)
        {
            this.dbContext = dbContext;
            this.toDomainConverter = toDomainConverter;
        }

        public async Task<IEnumerable<IStoreItemSection>> FindByAsync(IEnumerable<StoreItemSectionId> storeItemSectionIds,
            CancellationToken cancellationToken)
        {
            var sectionIds = storeItemSectionIds.Select(s => s.Value);

            var entities = await dbContext.Sections.AsNoTracking()
                .Where(section => sectionIds.Contains(section.Id))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return toDomainConverter.ToDomain(entities);
        }

        public async Task<IStoreItemSection> FindByAsync(StoreItemSectionId storeItemSectionId,
            CancellationToken cancellationToken)
        {
            var entity = await dbContext.Sections.AsNoTracking()
                .FirstOrDefaultAsync(section => section.Id == storeItemSectionId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
                return null;

            return toDomainConverter.ToDomain(entity);
        }
    }
}