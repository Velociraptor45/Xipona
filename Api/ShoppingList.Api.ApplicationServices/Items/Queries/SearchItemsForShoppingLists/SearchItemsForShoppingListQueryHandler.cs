using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItemsForShoppingLists;

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