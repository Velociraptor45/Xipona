using Microsoft.EntityFrameworkCore;
using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Infrastructure.Entities;
using ShoppingList.Api.Infrastructure.Extensions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Infrastructure.Adapters
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ShoppingContext dbContext;

        public ManufacturerRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Models.Manufacturer>> FindByAsync(string searchInput,
            CancellationToken cancellationToken)
        {
            var manufacturerEntities = await dbContext.Manufacturers.AsNoTracking()
                .Where(category => category.Name.Contains(searchInput))
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return manufacturerEntities.Select(entity => entity.ToDomain());
        }

        public async Task<Models.Manufacturer> FindByAsync(ManufacturerId id,
            CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = await dbContext.Manufacturers.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id.Value);

            if (entity == null)
                throw new ManufacturerNotFoundException($"Manufacturer {id.Value} not found.");

            cancellationToken.ThrowIfCancellationRequested();

            return entity.ToDomain();
        }

        public async Task<IEnumerable<Models.Manufacturer>> FindByAsync(IEnumerable<ManufacturerId> ids,
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

            return entities.Select(e => e.ToDomain());
        }

        public async Task<IEnumerable<Models.Manufacturer>> FindByAsync(bool includeDeleted,
            CancellationToken cancellationToken)
        {
            var results = await dbContext.Manufacturers.AsNoTracking()
                .Where(m => !m.Deleted || includeDeleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return results.Select(m => m.ToDomain());
        }
    }
}