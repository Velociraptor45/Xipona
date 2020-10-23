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
                throw new ManufacturerNotFoundException($"Manufacturer {id.Value} not found."); // todo

            return entity.ToDomain();
        }
    }
}