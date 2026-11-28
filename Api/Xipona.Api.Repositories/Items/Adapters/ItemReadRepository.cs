using Microsoft.EntityFrameworkCore;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Repositories.Items.Contexts;
using Xipona.Api.Repositories.Items.Entities;

namespace Xipona.Api.Repositories.Items.Adapters;

public class ItemReadRepository(
    ItemContext dbContext,
    IToDomainConverter<ShoppingListItemViewModel, SearchItemForShoppingResultReadModel> shoppingListItemConverter,
    CancellationToken ct)
    : IItemReadRepository
{
    public async Task<IEnumerable<SearchItemForShoppingResultReadModel>> GetShoppingListResultsAsync(string searchInput,
        StoreId storeId, int? limit)
    {
        var query = dbContext.ShoppingListItems
            .Where(item =>
                (EF.Functions.ILike(item.ItemName, $"%{searchInput}%")|| EF.Functions.ILike(item.ItemCategoryName, $"%{searchInput}%"))
                && item.StoreId == storeId.Value);
        
        if (limit.HasValue)
            query = query.Take(limit.Value);

        var entities = await query.ToListAsync(ct);

        return shoppingListItemConverter.ToDomain(entities);
    }
}