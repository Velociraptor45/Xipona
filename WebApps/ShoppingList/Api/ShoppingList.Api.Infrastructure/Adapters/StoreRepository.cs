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
    public class StoreRepository : IStoreRepository
    {
        private readonly ShoppingContext dbContext;

        public StoreRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Domain.Models.Store>> FindActiveStoresAsync(CancellationToken cancellationToken)
        {
            var storeEntities = await dbContext.Stores.AsNoTracking()
                .Where(store => !store.Deleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return storeEntities.Select(store => store.ToDomain());
        }

        public async Task<Domain.Models.Store> FindByAsync(StoreId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            cancellationToken.ThrowIfCancellationRequested();

            var entity = await dbContext.Stores.AsNoTracking()
                .FirstOrDefaultAsync(store => store.Id == id.Value);

            if (entity == null)
                throw new StoreNotFoundException(id);

            cancellationToken.ThrowIfCancellationRequested();

            return entity.ToDomain();
        }

        public async Task<bool> IsValidIdAsync(StoreId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            cancellationToken.ThrowIfCancellationRequested();

            var entity = await dbContext.Stores.AsNoTracking()
                .FirstOrDefaultAsync(store => store.Id == id.Value);

            cancellationToken.ThrowIfCancellationRequested();

            return entity != null;
        }

        public async Task<StoreId> StoreAsync(Domain.Models.Store store, CancellationToken cancellationToken)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            var storeEntity = store.ToEntity();

            cancellationToken.ThrowIfCancellationRequested();

            if (store.Id.Value == 0)
            {
                dbContext.Entry(storeEntity).State = EntityState.Added;
            }
            else
            {
                dbContext.Entry(storeEntity).State = EntityState.Modified;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();
            return new StoreId(storeEntity.Id);
        }
    }
}