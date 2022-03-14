using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItems;

public class SearchItemQueryHandler : IQueryHandler<SearchItemQuery, IEnumerable<SearchItemResultReadModel>>
{
    private readonly Func<CancellationToken, IItemSearchService> _itemQueryServiceDelegate;

    public SearchItemQueryHandler(Func<CancellationToken, IItemSearchService> itemSearchServiceDelegate)
    {
        _itemQueryServiceDelegate = itemSearchServiceDelegate;
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> HandleAsync(SearchItemQuery query,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var itemSearchService = _itemQueryServiceDelegate(cancellationToken);
        return await itemSearchService.SearchAsync(query.SearchInput);
    }
}