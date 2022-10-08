using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Adapters;

public class StoreRepository : IStoreRepository
{
    private readonly StoreContext _dbContext;
    private readonly IToDomainConverter<Entities.Store, IStore> _toDomainConverter;
    private readonly IToEntityConverter<IStore, Entities.Store> _toEntityConverter;

    public StoreRepository(StoreContext dbContext,
        IToDomainConverter<Entities.Store, IStore> toDomainConverter,
        IToEntityConverter<IStore, Entities.Store> toEntityConverter)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toEntityConverter = toEntityConverter;
    }

    public async Task<IEnumerable<IStore>> GetAsync(CancellationToken cancellationToken)
    {
        var storeEntities = await GetStoreQuery()
            .Where(store => !store.Deleted)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toDomainConverter.ToDomain(storeEntities);
    }

    public async Task<IStore?> FindActiveByAsync(StoreId id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await GetStoreQuery()
            .Where(store => !store.Deleted)
            .FirstOrDefaultAsync(store => store.Id == id.Value, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IStore>> FindActiveByAsync(IEnumerable<StoreId> ids,
        CancellationToken cancellationToken)
    {
        return await FindByAsync(ids, true, cancellationToken);
    }

    public async Task<IStore?> FindByAsync(StoreId id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await GetStoreQuery()
            .FirstOrDefaultAsync(store => store.Id == id.Value, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return entity == null ? null : _toDomainConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IStore>> FindByAsync(IEnumerable<StoreId> ids, bool onlyActives,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var idsList = ids.Select(id => id.Value).ToList();

        var query = GetStoreQuery()
            .Where(store => idsList.Contains(store.Id));

        if (onlyActives)
            query = query.Where(s => !s.Deleted);

        var entities = await query.ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IStore> StoreAsync(IStore store, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var existingEntity = await FindEntityById(store.Id.Value, cancellationToken);

        if (existingEntity is null)
        {
            return await StoreAsNew(store, cancellationToken);
        }
        var existingSections = existingEntity.Sections.ToDictionary(s => s.Id);
        var incomingEntity = _toEntityConverter.ToEntity(store);

        _dbContext.Entry(incomingEntity).State = EntityState.Modified;

        foreach (var section in incomingEntity.Sections)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (existingSections.ContainsKey(section.Id))
            {
                // section was modified
                _dbContext.Entry(section).State = EntityState.Modified;
                existingSections.Remove(section.Id);
            }
            else
            {
                // section was added
                _dbContext.Entry(section).State = EntityState.Added;
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var section in existingSections.Values)
        {
            _dbContext.Entry(section).State = EntityState.Deleted;
        }

        cancellationToken.ThrowIfCancellationRequested();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _toDomainConverter.ToDomain(incomingEntity);
    }

    #region private methods

    private async Task<IStore> StoreAsNew(IStore store, CancellationToken cancellationToken)
    {
        var entity = _toEntityConverter.ToEntity(store);
        _dbContext.Entry(entity).State = EntityState.Added;

        foreach (var section in entity.Sections)
        {
            _dbContext.Entry(section).State = EntityState.Added;
        }

        cancellationToken.ThrowIfCancellationRequested();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _toDomainConverter.ToDomain(entity);
    }

    private async Task<Entities.Store?> FindEntityById(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _dbContext.Stores.AsNoTracking()
            .Include(s => s.Sections)
            .Where(store => !store.Deleted)
            .FirstOrDefaultAsync(store => store.Id == id, cancellationToken);
    }

    private IQueryable<Entities.Store> GetStoreQuery()
    {
        return _dbContext.Stores.AsNoTracking()
            .Include(s => s.Sections);
    }

    #endregion private methods
}