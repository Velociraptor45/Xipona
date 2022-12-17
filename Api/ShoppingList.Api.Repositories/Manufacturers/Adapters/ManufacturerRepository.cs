using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Contexts;

namespace ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Adapters;

public class ManufacturerRepository : IManufacturerRepository
{
    private readonly ManufacturerContext _dbContext;
    private readonly IToDomainConverter<Entities.Manufacturer, IManufacturer> _toModelConverter;
    private readonly IToEntityConverter<IManufacturer, Entities.Manufacturer> _toEntityConverter;

    public ManufacturerRepository(ManufacturerContext dbContext,
        IToDomainConverter<Entities.Manufacturer, IManufacturer> toModelConverter,
        IToEntityConverter<IManufacturer, Entities.Manufacturer> toEntityConverter)
    {
        _dbContext = dbContext;
        _toModelConverter = toModelConverter;
        _toEntityConverter = toEntityConverter;
    }

    public async Task<IEnumerable<IManufacturer>> FindByAsync(string searchInput, bool includeDeleted,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Manufacturers.AsNoTracking()
            .Where(manufacturer => manufacturer.Name.Contains(searchInput));

        if (!includeDeleted)
            query = query.Where(manufacturer => !manufacturer.Deleted);

        var manufacturerEntities = await query.ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(manufacturerEntities);
    }

    public async Task<IManufacturer?> FindByAsync(ManufacturerId id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Manufacturers.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IManufacturer>> FindByAsync(IEnumerable<ManufacturerId> ids,
        CancellationToken cancellationToken)
    {
        var idHashSet = ids.Select(id => id.Value).ToHashSet();

        cancellationToken.ThrowIfCancellationRequested();

        var entities = await _dbContext.Manufacturers.AsNoTracking()
            .Where(m => idHashSet.Contains(m.Id))
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IManufacturer>> FindActiveByAsync(CancellationToken cancellationToken)
    {
        var entities = await _dbContext.Manufacturers.AsNoTracking()
            .Where(m => !m.Deleted)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IManufacturer?> FindActiveByAsync(ManufacturerId id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Manufacturers.AsNoTracking()
            .FirstOrDefaultAsync(m => !m.Deleted && m.Id == id, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IManufacturer> StoreAsync(IManufacturer model, CancellationToken cancellationToken)
    {
        var convertedEntity = _toEntityConverter.ToEntity(model);
        var existingEntity = await FindTrackedEntityById(model.Id, cancellationToken);

        if (existingEntity is null)
        {
            _dbContext.Entry(convertedEntity).State = EntityState.Added;
        }
        else
        {
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(convertedEntity);
            _dbContext.Entry(existingEntity).State = EntityState.Modified;
        }

        cancellationToken.ThrowIfCancellationRequested();

        await _dbContext.SaveChangesAsync(cancellationToken);
        return _toModelConverter.ToDomain(convertedEntity);
    }

    private async Task<Entities.Manufacturer?> FindTrackedEntityById(ManufacturerId id, CancellationToken cancellationToken)
    {
        return await _dbContext.Manufacturers
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }
}