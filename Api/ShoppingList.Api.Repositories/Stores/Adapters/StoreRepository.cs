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
    private readonly IToContractConverter<IStore, Entities.Store> _toContractConverter;
    private readonly ILogger<StoreRepository> _logger;

    public StoreRepository(StoreContext dbContext,
        IToDomainConverter<Entities.Store, IStore> toDomainConverter,
        IToContractConverter<IStore, Entities.Store> toContractConverter,
        ILogger<StoreRepository> logger)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toContractConverter = toContractConverter;
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

        var existingEntity = await FindTrackedEntityById(store.Id, cancellationToken);

        if (existingEntity is null)
        {
            return await StoreAsNew(store, cancellationToken);
        }

        var updatedEntity = _toContractConverter.ToContract(store);

        var existingRowVersion = existingEntity.RowVersion;
        _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
        _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
        _dbContext.Entry(existingEntity).State = EntityState.Modified;

        AddOrUpdateSections(existingEntity, updatedEntity);
        DeleteSections(existingEntity, updatedEntity);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex, () => $"Saving store {store.Id.Value} failed due to concurrency violation");
            throw new DomainException(new ModelOutOfDateReason());
        }

        return _toDomainConverter.ToDomain(existingEntity);
    }

    #region private methods

    private void AddOrUpdateSections(Entities.Store existingStore, Entities.Store updatedStore)
    {
        foreach (var updatedSection in updatedStore.Sections)
        {
            var existingSection = existingStore.Sections
                .FirstOrDefault(t => t.Id == updatedSection.Id);

            if (existingSection == null)
            {
                existingStore.Sections.Add(updatedSection);
            }
            else
            {
                _dbContext.Entry(existingSection).CurrentValues.SetValues(updatedSection);
            }
        }
    }

    private void DeleteSections(Entities.Store existingStore, Entities.Store updatedStore)
    {
        var deletedSections = existingStore.Sections
            .Where(t => updatedStore.Sections.All(s => s.Id != t.Id))
            .ToList();

        foreach (var deletedSection in deletedSections)
        {
            _dbContext.Remove(deletedSection);
        }
    }

    private async Task<IStore> StoreAsNew(IStore store, CancellationToken cancellationToken)
    {
        var entity = _toContractConverter.ToContract(store);
        _dbContext.Entry(entity).State = EntityState.Added;

        foreach (var section in entity.Sections)
        {
            _dbContext.Entry(section).State = EntityState.Added;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _toDomainConverter.ToDomain(entity);
    }

    private async Task<Entities.Store?> FindTrackedEntityById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Stores
            .Include(s => s.Sections)
            .FirstOrDefaultAsync(store => store.Id == id, cancellationToken);
    }

    private IQueryable<Entities.Store> GetStoreQuery()
    {
        return _dbContext.Stores.AsNoTracking()
            .Include(s => s.Sections);
    }

    #endregion private methods
}