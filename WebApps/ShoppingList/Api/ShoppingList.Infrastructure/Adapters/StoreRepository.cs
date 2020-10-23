using Microsoft.EntityFrameworkCore;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Ports;
using ShoppingList.Infrastructure.Converters;
using ShoppingList.Infrastructure.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

using Models = ShoppingList.Domain.Models;

namespace ShoppingList.Infrastructure.Adapters
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ShoppingContext dbContext;

        public StoreRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<StoreId> StoreAsync(Models.Store store, CancellationToken cancellationToken)
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