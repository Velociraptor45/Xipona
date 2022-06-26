using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Adapters;

public class ItemCategoryRepository : IItemCategoryRepository
{
    private readonly ItemCategoryContext _dbContext;
    private readonly IToDomainConverter<Entities.ItemCategory, IItemCategory> _toModelConverter;
    private readonly IToEntityConverter<IItemCategory, Entities.ItemCategory> _toEntityConverter;

    public ItemCategoryRepository(ItemCategoryContext dbContext,
        IToDomainConverter<Entities.ItemCategory, IItemCategory> toModelConverter,
        IToEntityConverter<IItemCategory, Entities.ItemCategory> toEntityConverter)
    {
        _dbContext = dbContext;
        _toModelConverter = toModelConverter;
        _toEntityConverter = toEntityConverter;
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
            .FirstOrDefaultAsync(category => category.Id == id.Value, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids,
        CancellationToken cancellationToken)
    {
        if (ids == null)
            throw new ArgumentNullException(nameof(ids));

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

    public async Task<IItemCategory> StoreAsync(IItemCategory model, CancellationToken cancellationToken)
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

    private async Task<Entities.ItemCategory?> FindTrackedEntityById(ItemCategoryId id, CancellationToken cancellationToken)
    {
        return await _dbContext.ItemCategories
            .FirstOrDefaultAsync(i => i.Id == id.Value, cancellationToken);
    }
}