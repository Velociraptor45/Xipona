using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
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
    public class StoreRepository : IStoreRepository
    {
        private readonly ShoppingContext dbContext;

        public StoreRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<IStore>> GetAsync(CancellationToken cancellationToken)
        {
            var storeEntities = await dbContext.Stores.AsNoTracking()
                .Include(s => s.Sections)
                .Where(store => !store.Deleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return storeEntities.Select(store => store.ToDomain());
        }

        public async Task<IStore> FindActiveByAsync(StoreId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            cancellationToken.ThrowIfCancellationRequested();

            var entity = await dbContext.Stores.AsNoTracking()
                .Include(s => s.Sections)
                .Where(store => !store.Deleted)
                .FirstOrDefaultAsync(store => store.Id == id.Value);

            if (entity == null) // todo: move to command handler
                throw new DomainException(new StoreNotFoundReason(id));

            cancellationToken.ThrowIfCancellationRequested();

            return entity.ToDomain();
        }

        public async Task<IStore> FindByAsync(StoreId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            cancellationToken.ThrowIfCancellationRequested();

            var entity = await dbContext.Stores.AsNoTracking()
                .Include(s => s.Sections)
                .FirstOrDefaultAsync(store => store.Id == id.Value);

            if (entity == null) // todo: move to command handler
                throw new DomainException(new StoreNotFoundReason(id));

            cancellationToken.ThrowIfCancellationRequested();

            return entity.ToDomain();
        }

        public async Task StoreAsync(IStore store, CancellationToken cancellationToken)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            cancellationToken.ThrowIfCancellationRequested();

            if (store.Id.Value == 0)
            {
                await StoreAsNew(store, cancellationToken);
            }
            else
            {
                await StoreAsModified(store, cancellationToken);
            }
        }

        #region private methods

        private async Task StoreAsNew(IStore store, CancellationToken cancellationToken)
        {
            var entity = store.ToEntity();
            dbContext.Entry(entity).State = EntityState.Added;

            foreach (var section in entity.Sections)
            {
                dbContext.Entry(section).State = EntityState.Added;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();
        }

        private async Task StoreAsModified(IStore store, CancellationToken cancellationToken)
        {
            var existingEntity = await FindEntityById(store.Id.Value, cancellationToken);
            var existingSections = existingEntity.Sections.ToDictionary(s => s.Id);
            var incomingEntity = store.ToEntity();

            dbContext.Entry(incomingEntity).State = EntityState.Modified;

            foreach (var section in incomingEntity.Sections)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (existingSections.ContainsKey(section.Id))
                {
                    // section was modified
                    dbContext.Entry(section).State = EntityState.Modified;
                    existingSections.Remove(section.Id);
                }
                else
                {
                    // section was added
                    dbContext.Entry(section).State = EntityState.Added;
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var section in existingSections.Values)
            {
                dbContext.Entry(section).State = EntityState.Deleted;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();
        }

        private async Task<Entities.Store> FindEntityById(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await dbContext.Stores.AsNoTracking()
                .Include(s => s.Sections)
                .Where(store => !store.Deleted)
                .FirstOrDefaultAsync(store => store.Id == id);
        }

        #endregion private methods
    }
}