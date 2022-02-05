using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Adapters;

public class ItemCategoryRepository : IItemCategoryRepository
{
    private readonly ItemCategoryContext dbContext;
    private readonly IToDomainConverter<Entities.ItemCategory, IItemCategory> toModelConverter;
    private readonly IToEntityConverter<IItemCategory, Entities.ItemCategory> toEntityConverter;

    public ItemCategoryRepository(ItemCategoryContext dbContext,
        IToDomainConverter<Entities.ItemCategory, IItemCategory> toModelConverter,
        IToEntityConverter<IItemCategory, Entities.ItemCategory> toEntityConverter)
    {
        this.dbContext = dbContext;
        this.toModelConverter = toModelConverter;
        this.toEntityConverter = toEntityConverter;
    }

    public async Task<IEnumerable<IItemCategory>> FindByAsync(string searchInput,
        CancellationToken cancellationToken)
    {
        var itemCategoryEntities = await dbContext.ItemCategories.AsNoTracking()
            .Where(category => category.Name.Contains(searchInput))
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return toModelConverter.ToDomain(itemCategoryEntities);
    }

    public async Task<IItemCategory?> FindByAsync(ItemCategoryId id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.ItemCategories.AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id.Value, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (entity == null)
            return null;

        return toModelConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IItemCategory>> FindByAsync(IEnumerable<ItemCategoryId> ids,
        CancellationToken cancellationToken)
    {
        if (ids == null)
            throw new ArgumentNullException(nameof(ids));

        var idHashSet = ids.Select(id => id.Value).ToHashSet();

        cancellationToken.ThrowIfCancellationRequested();

        var entities = await dbContext.ItemCategories.AsNoTracking()
            .Where(m => idHashSet.Contains(m.Id))
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItemCategory>> FindActiveByAsync(CancellationToken cancellationToken)
    {
        var entities = await dbContext.ItemCategories.AsNoTracking()
            .Where(m => !m.Deleted)
            .ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return toModelConverter.ToDomain(entities);
    }

    public async Task<IItemCategory> StoreAsync(IItemCategory model,
        CancellationToken cancellationToken)
    {
        var entity = toEntityConverter.ToEntity(model);

        dbContext.Entry(entity).State = entity.Id <= 0 ? EntityState.Added : EntityState.Modified;

        cancellationToken.ThrowIfCancellationRequested();

        await dbContext.SaveChangesAsync(cancellationToken);
        return toModelConverter.ToDomain(entity);
    }
}