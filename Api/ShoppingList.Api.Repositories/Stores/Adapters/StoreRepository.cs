using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Contexts;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Adapters;

public class StoreRepository : IStoreRepository
{
    private readonly StoreContext _dbContext;
    private readonly IToDomainConverter<Entities.Store, IStore> _toDomainConverter;
    private readonly IToEntityConverter<IStore, Entities.Store> _toEntityConverter;
    private readonly ILogger<StoreRepository> _logger;

    public StoreRepository(StoreContext dbContext,
        IToDomainConverter<Entities.Store, IStore> toDomainConverter,
        IToEntityConverter<IStore, Entities.Store> toEntityConverter,
        ILogger<StoreRepository> logger)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toEntityConverter = toEntityConverter;
        _logger = logger;
    }

    public async Task<IStore?> FindByAsync(StoreId id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await GetStoreQuery()
            .FirstOrDefaultAsync(store => store.Id == id, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return entity == null ? null : _toDomainConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IStore>> GetActiveAsync(CancellationToken cancellationToken)
    {
        var storeEntities = await GetStoreQuery()
            .Where(store => !store.Deleted)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toDomainConverter.ToDomain(storeEntities);
    }

    public async Task<IStore?> FindActiveByAsync(SectionId sectionId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await GetStoreQuery()
            .Where(store => !store.Deleted)
            .FirstOrDefaultAsync(store => store.Sections.Any(s => s.Id == sectionId), cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    public async Task<IStore?> FindActiveByAsync(StoreId id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await GetStoreQuery()
            .Where(store => !store.Deleted)
            .FirstOrDefaultAsync(store => store.Id == id, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IStore>> FindActiveByAsync(IEnumerable<StoreId> ids,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var idsList = ids.Select(id => id.Value).ToList();

        var query = GetStoreQuery()
            .Where(store => !store.Deleted && idsList.Contains(store.Id));

        var entities = await query.ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IStore> StoreAsync(IStore store, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var existingEntity = await FindEntityById(store.Id, cancellationToken);

        if (existingEntity is null)
        {
            return await StoreAsNew(store, cancellationToken);
        }
        var existingSections = existingEntity.Sections.ToDictionary(s => s.Id);
        var updatedEntity = _toEntityConverter.ToEntity(store);

        var existingRowVersion = existingEntity.RowVersion;
        _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
        _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
        _dbContext.Entry(existingEntity).State = EntityState.Modified;

        foreach (var section in updatedEntity.Sections)
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

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex, () => $"Saving store {store.Id.Value} failed due to concurrency violation");
            throw new DomainException(new ModelOutOfDateReason());
        }

        return _toDomainConverter.ToDomain(updatedEntity);
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