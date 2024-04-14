﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.Repositories.Manufacturers.Contexts;

namespace ProjectHermes.Xipona.Api.Repositories.Manufacturers.Adapters;

public class ManufacturerRepository : IManufacturerRepository
{
    private readonly ManufacturerContext _dbContext;
    private readonly IToDomainConverter<Entities.Manufacturer, IManufacturer> _toModelConverter;
    private readonly IToEntityConverter<IManufacturer, Entities.Manufacturer> _toEntityConverter;
    private readonly ILogger<ManufacturerRepository> _logger;
    private readonly CancellationToken _cancellationToken;

    public ManufacturerRepository(ManufacturerContext dbContext,
        IToDomainConverter<Entities.Manufacturer, IManufacturer> toModelConverter,
        IToEntityConverter<IManufacturer, Entities.Manufacturer> toEntityConverter,
        ILogger<ManufacturerRepository> logger,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _toModelConverter = toModelConverter;
        _toEntityConverter = toEntityConverter;
        _logger = logger;
        _cancellationToken = cancellationToken;
    }

    public async Task<IEnumerable<IManufacturer>> FindByAsync(string searchInput, bool includeDeleted)
    {
        var query = _dbContext.Manufacturers.AsNoTracking()
            .Where(manufacturer => manufacturer.Name.Contains(searchInput));

        if (!includeDeleted)
            query = query.Where(manufacturer => !manufacturer.Deleted);

        var manufacturerEntities = await query.ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(manufacturerEntities);
    }

    public async Task<IManufacturer?> FindByAsync(ManufacturerId id)
    {
        var entity = await _dbContext.Manufacturers.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, _cancellationToken);

        if (entity == null)
            return null;

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IManufacturer>> FindByAsync(IEnumerable<ManufacturerId> ids)
    {
        var idHashSet = ids.Select(id => id.Value).ToHashSet();

        var entities = await _dbContext.Manufacturers.AsNoTracking()
            .Where(m => idHashSet.Contains(m.Id))
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IManufacturer>> FindActiveByAsync()
    {
        var entities = await _dbContext.Manufacturers.AsNoTracking()
            .Where(m => !m.Deleted)
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IManufacturer?> FindActiveByAsync(ManufacturerId id)
    {
        var entity = await _dbContext.Manufacturers.AsNoTracking()
            .FirstOrDefaultAsync(m => !m.Deleted && m.Id == id, _cancellationToken);

        if (entity == null)
            return null;

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IManufacturer> StoreAsync(IManufacturer model)
    {
        var convertedEntity = _toEntityConverter.ToEntity(model);
        var existingEntity = await FindTrackedEntityById(model.Id);

        if (existingEntity is null)
        {
            _dbContext.Entry(convertedEntity).State = EntityState.Added;
        }
        else
        {
            var existingRowVersion = existingEntity.RowVersion;
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(convertedEntity);
            _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
            _dbContext.Entry(existingEntity).State = EntityState.Modified;
        }

        try
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex,
                () => "Saving manufacturer '{ManufacturerId}' failed due to concurrency violation", model.Id.Value);
            throw new DomainException(new ModelOutOfDateReason());
        }
        return _toModelConverter.ToDomain(convertedEntity);
    }

    private async Task<Entities.Manufacturer?> FindTrackedEntityById(ManufacturerId id)
    {
        return await _dbContext.Manufacturers.FirstOrDefaultAsync(i => i.Id == id, _cancellationToken);
    }
}