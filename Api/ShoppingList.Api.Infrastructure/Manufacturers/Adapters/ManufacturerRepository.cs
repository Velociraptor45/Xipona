using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Adapters
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ManufacturerContext dbContext;
        private readonly IToDomainConverter<Entities.Manufacturer, IManufacturer> toModelConverter;
        private readonly IToEntityConverter<IManufacturer, Entities.Manufacturer> toEntityConverter;

        public ManufacturerRepository(ManufacturerContext dbContext,
            IToDomainConverter<Entities.Manufacturer, IManufacturer> toModelConverter,
            IToEntityConverter<IManufacturer, Entities.Manufacturer> toEntityConverter)
        {
            this.dbContext = dbContext;
            this.toModelConverter = toModelConverter;
            this.toEntityConverter = toEntityConverter;
        }

        public async Task<IEnumerable<IManufacturer>> FindByAsync(string searchInput,
            CancellationToken cancellationToken)
        {
            var manufacturerEntities = await dbContext.Manufacturers.AsNoTracking()
                .Where(category => category.Name.Contains(searchInput))
                .ToListAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return toModelConverter.ToDomain(manufacturerEntities);
        }

        public async Task<IManufacturer?> FindByAsync(ManufacturerId id,
            CancellationToken cancellationToken)
        {
            var entity = await dbContext.Manufacturers.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id.Value, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
                return null;

            return toModelConverter.ToDomain(entity);
        }

        public async Task<IEnumerable<IManufacturer>> FindByAsync(IEnumerable<ManufacturerId> ids,
            CancellationToken cancellationToken)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var idHashSet = ids.Select(id => id.Value).ToHashSet();

            cancellationToken.ThrowIfCancellationRequested();

            var entities = await dbContext.Manufacturers.AsNoTracking()
                .Where(m => idHashSet.Contains(m.Id))
                .ToListAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return toModelConverter.ToDomain(entities);
        }

        public async Task<IEnumerable<IManufacturer>> FindByAsync(bool includeDeleted,
            CancellationToken cancellationToken)
        {
            var entities = await dbContext.Manufacturers.AsNoTracking()
                .Where(m => !m.Deleted || includeDeleted)
                .ToListAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return toModelConverter.ToDomain(entities);
        }

        public async Task<IManufacturer> StoreAsync(IManufacturer model, CancellationToken cancellationToken)
        {
            var entity = toEntityConverter.ToEntity(model);

            dbContext.Entry(entity).State = entity.Id <= 0 ? EntityState.Added : EntityState.Modified;

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync(cancellationToken);
            return toModelConverter.ToDomain(entity);
        }
    }
}