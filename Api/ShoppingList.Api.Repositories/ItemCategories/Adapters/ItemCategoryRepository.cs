using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Contexts;

namespace ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Adapters;

public class ItemCategoryRepository : IItemCategoryRepository
{
    private readonly ItemCategoryContext _dbContext;
    private readonly IToDomainConverter<Entities.ItemCategory, IItemCategory> _toModelConverter;
    private readonly IToContractConverter<IItemCategory, Entities.ItemCategory> _toContractConverter;
    private readonly ILogger<ItemCategoryRepository> _logger;

    public ItemCategoryRepository(ItemCategoryContext dbContext,
        IToDomainConverter<Entities.ItemCategory, IItemCategory> toModelConverter,
        IToContractConverter<IItemCategory, Entities.ItemCategory> toContractConverter,
        ILogger<ItemCategoryRepository> logger)
    {
        _dbContext = dbContext;
        _toModelConverter = toModelConverter;
        _toContractConverter = toContractConverter;
        _logger = logger;
    }

    public async Task<IEnumerable<IItemCategory>> FindByAsync(string searchInput, bool includeDeleted,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.ItemCategories.AsNoTracking()
            .Where(category => category.Name.Contains(searchInput));

        if (!includeDeleted)
            query = query.Where(category => !category.Deleted);

        cancellationToken.ThrowIfCancellationRequested();

        var itemCategoryEntities = await query.ToListAsync(cancellationToken);

        return _toModelConverter.ToDomain(itemCategoryEntities);
    }

    public async Task<IItemCategory?> FindByAsync(ItemCategoryId id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.ItemCategories.AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids,
        CancellationToken cancellationToken)
    {
        var idHashSet = ids.Select(id => id.Value).ToHashSet();

        cancellationToken.ThrowIfCancellationRequested();

        var entities = await _dbContext.ItemCategories.AsNoTracking()
            .Where(m => idHashSet.Contains(m.Id))
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItemCategory>> FindActiveByAsync(CancellationToken cancellationToken)
    {
        var entities = await _dbContext.ItemCategories.AsNoTracking()
            .Where(m => !m.Deleted)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IItemCategory?> FindActiveByAsync(ItemCategoryId id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.ItemCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => !category.Deleted && category.Id == id, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IItemCategory> StoreAsync(IItemCategory model, CancellationToken cancellationToken)
    {
        var convertedEntity = _toContractConverter.ToContract(model);
        var existingEntity = await FindTrackedEntityById(model.Id, cancellationToken);

        if (existingEntity is null)
        {
            _dbContext.Entry(convertedEntity).State = EntityState.Added;
        }
        else
        {
            var existingRowVersion = existingEntity.RowVersion;
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(convertedEntity);
            _dbContext.Entry(existingEntity).State = EntityState.Modified;
            _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
        }

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex,
                () => $"Saving item category {model.Id.Value} failed due to concurrency violation");
            throw new DomainException(new ModelOutOfDateReason());
        }
        return _toModelConverter.ToDomain(convertedEntity);
    }

    private async Task<Entities.ItemCategory?> FindTrackedEntityById(ItemCategoryId id, CancellationToken cancellationToken)
    {
        return await _dbContext.ItemCategories
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }
}