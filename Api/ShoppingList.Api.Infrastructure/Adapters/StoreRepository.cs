using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
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
        private readonly IToDomainConverter<Entities.Store, IStore> toDomainConverter;
        private readonly IToEntityConverter<IStore, Entities.Store> toEntityConverter;

        public StoreRepository(ShoppingContext dbContext,
            IToDomainConverter<Entities.Store, IStore> toDomainConverter,
            IToEntityConverter<IStore, Entities.Store> toEntityConverter)
        {
            this.dbContext = dbContext;
            this.toDomainConverter = toDomainConverter;
            this.toEntityConverter = toEntityConverter;
        }

        public async Task<IEnumerable<IStore>> GetAsync(CancellationToken cancellationToken)
        {
            var storeEntities = await GetStoreQuery()
                .Where(store => !store.Deleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return toDomainConverter.ToDomain(storeEntities);
        }

        public async Task<IStore> FindActiveByAsync(StoreId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            cancellationToken.ThrowIfCancellationRequested();

            var entity = await GetStoreQuery()
                .Where(store => !store.Deleted)
                .FirstOrDefaultAsync(store => store.Id == id.Value);

            cancellationToken.ThrowIfCancellationRequested();

            return toDomainConverter.ToDomain(entity);
        }

        public async Task<IStore> FindByAsync(StoreId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            cancellationToken.ThrowIfCancellationRequested();

            var entity = await GetStoreQuery()
                .FirstOrDefaultAsync(store => store.Id == id.Value);

            cancellationToken.ThrowIfCancellationRequested();

            return toDomainConverter.ToDomain(entity);
        }

        public async Task<IStore> StoreAsync(IStore store, CancellationToken cancellationToken)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            cancellationToken.ThrowIfCancellationRequested();

            if (store.Id.Value == 0)
            {
                return await StoreAsNew(store, cancellationToken);
            }
            else
            {
                return await StoreAsModified(store, cancellationToken);
            }
        }

        #region private methods

        private async Task<IStore> StoreAsNew(IStore store, CancellationToken cancellationToken)
        {
            var entity = toEntityConverter.ToEntity(store);
            dbContext.Entry(entity).State = EntityState.Added;

            foreach (var section in entity.Sections)
            {
                dbContext.Entry(section).State = EntityState.Added;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();

            return toDomainConverter.ToDomain(entity);
        }

        private async Task<IStore> StoreAsModified(IStore store, CancellationToken cancellationToken)
        {
            var existingEntity = await FindEntityById(store.Id.Value, cancellationToken);
            var existingSections = existingEntity.Sections.ToDictionary(s => s.Id);
            var incomingEntity = toEntityConverter.ToEntity(store);

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

            return toDomainConverter.ToDomain(incomingEntity);
        }

        private async Task<Entities.Store> FindEntityById(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await dbContext.Stores.AsNoTracking()
                .Include(s => s.Sections)
                .Where(store => !store.Deleted)
                .FirstOrDefaultAsync(store => store.Id == id);
        }

        private IQueryable<Entities.Store> GetStoreQuery()
        {
            return dbContext.Stores.AsNoTracking()
                .Include(s => s.Sections);
        }

        #endregion private methods
    }
}