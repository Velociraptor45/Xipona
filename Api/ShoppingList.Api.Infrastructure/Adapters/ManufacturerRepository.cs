using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
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
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ShoppingContext dbContext;
        private readonly IToDomainConverter<Entities.Manufacturer, IManufacturer> manufacturerConverter;

        public ManufacturerRepository(ShoppingContext dbContext,
            IToDomainConverter<Entities.Manufacturer, IManufacturer> manufacturerConverter)
        {
            this.dbContext = dbContext;
            this.manufacturerConverter = manufacturerConverter;
        }

        public async Task<IEnumerable<IManufacturer>> FindByAsync(string searchInput,
            CancellationToken cancellationToken)
        {
            var manufacturerEntities = await dbContext.Manufacturers.AsNoTracking()
                .Where(category => category.Name.Contains(searchInput))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return manufacturerEntities.Select(entity => entity.ToDomain());
        }

        public async Task<IManufacturer> FindByAsync(ManufacturerId id,
            CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = await dbContext.Manufacturers.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id.Value);

            if (entity == null)
                throw new DomainException(new ManufacturerNotFoundReason(id));

            cancellationToken.ThrowIfCancellationRequested();

            return manufacturerConverter.ToDomain(entity);
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
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return manufacturerConverter.ToDomain(entities);
        }

        public async Task<IEnumerable<IManufacturer>> FindByAsync(bool includeDeleted,
            CancellationToken cancellationToken)
        {
            var entities = await dbContext.Manufacturers.AsNoTracking()
                .Where(m => !m.Deleted || includeDeleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return manufacturerConverter.ToDomain(entities);
        }

        public async Task<IManufacturer> StoreAsync(IManufacturer model, CancellationToken cancellationToken)
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
            return manufacturerConverter.ToDomain(entity);
        }
    }
}