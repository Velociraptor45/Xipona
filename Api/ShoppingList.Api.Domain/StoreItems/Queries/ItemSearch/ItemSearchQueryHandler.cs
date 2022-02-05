using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;

public class ItemSearchQueryHandler : IQueryHandler<ItemSearchQuery, IEnumerable<ItemSearchReadModel>>
{
    private readonly Func<CancellationToken, IItemQueryService> _itemQueryServiceDelegate;

    public ItemSearchQueryHandler(Func<CancellationToken, IItemQueryService> itemQueryServiceDelegate)
    {
        _itemQueryServiceDelegate = itemQueryServiceDelegate;
    }

    public async Task<IEnumerable<ItemSearchReadModel>> HandleAsync(ItemSearchQuery query,
        CancellationToken cancellationToken)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (string.IsNullOrWhiteSpace(query.SearchInput))
            return Enumerable.Empty<ItemSearchReadModel>();

        var itemQueryService = _itemQueryServiceDelegate(cancellationToken);
        return await itemQueryService.SearchAsync(query.SearchInput, query.StoreId);
    }
}