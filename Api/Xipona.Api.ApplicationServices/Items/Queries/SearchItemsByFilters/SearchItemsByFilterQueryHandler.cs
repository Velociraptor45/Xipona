﻿using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItemsByFilters;

public class SearchItemsByFilterQueryHandler : IQueryHandler<SearchItemsByFilterQuery, IEnumerable<SearchItemResultReadModel>>
{
    private readonly Func<CancellationToken, IItemSearchService> _itemQueryServiceDelegate;

    public SearchItemsByFilterQueryHandler(Func<CancellationToken, IItemSearchService> itemSearchServiceDelegate)
    {
        _itemQueryServiceDelegate = itemSearchServiceDelegate;
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> HandleAsync(
        SearchItemsByFilterQuery query, CancellationToken cancellationToken)
    {
        var itemSearchService = _itemQueryServiceDelegate(cancellationToken);
        return await itemSearchService.SearchAsync(query.StoreIds, query.ItemCategoriesIds,
            query.ManufacturerIds);
    }
}