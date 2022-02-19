using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Search;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItemsForShoppingLists;

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
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (string.IsNullOrWhiteSpace(query.SearchInput))
            return Enumerable.Empty<SearchItemForShoppingResultReadModel>();

        var itemSearchService = _itemQueryServiceDelegate(cancellationToken);
        return await itemSearchService.SearchForShoppingListAsync(query.SearchInput, query.StoreId);
    }
}