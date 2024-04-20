﻿using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForShoppingLists;

public class SearchItemsForShoppingListQueryHandler : IQueryHandler<SearchItemsForShoppingListQuery, IEnumerable<SearchItemForShoppingResultReadModel>>
{
    private readonly Func<CancellationToken, IItemSearchService> _itemQueryServiceDelegate;

    public SearchItemsForShoppingListQueryHandler(Func<CancellationToken, IItemSearchService> itemSearchServiceDelegate)
    {
        _itemQueryServiceDelegate = itemSearchServiceDelegate;
    }

    public async Task<IEnumerable<SearchItemForShoppingResultReadModel>> HandleAsync(SearchItemsForShoppingListQuery query,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.SearchInput))
            return Enumerable.Empty<SearchItemForShoppingResultReadModel>();

        var itemSearchService = _itemQueryServiceDelegate(cancellationToken);
        return await itemSearchService.SearchForShoppingListAsync(query.SearchInput, query.StoreId);
    }
}