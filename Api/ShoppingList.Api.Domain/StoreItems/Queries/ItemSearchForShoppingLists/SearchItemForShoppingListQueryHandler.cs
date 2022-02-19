using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearchForShoppingLists;

public class SearchItemForShoppingListQueryHandler : IQueryHandler<SearchItemForShoppingListQuery, IEnumerable<ItemForShoppingListSearchReadModel>>
{
    private readonly Func<CancellationToken, IItemQueryService> _itemQueryServiceDelegate;

    public SearchItemForShoppingListQueryHandler(Func<CancellationToken, IItemQueryService> itemQueryServiceDelegate)
    {
        _itemQueryServiceDelegate = itemQueryServiceDelegate;
    }

    public async Task<IEnumerable<ItemForShoppingListSearchReadModel>> HandleAsync(SearchItemForShoppingListQuery query,
        CancellationToken cancellationToken)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (string.IsNullOrWhiteSpace(query.SearchInput))
            return Enumerable.Empty<ItemForShoppingListSearchReadModel>();

        var itemQueryService = _itemQueryServiceDelegate(cancellationToken);
        return await itemQueryService.SearchForShoppingListAsync(query.SearchInput, query.StoreId);
    }
}