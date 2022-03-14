using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Queries.ActiveShoppingListByStoreId;

public class ActiveShoppingListByStoreIdQueryHandler
    : IQueryHandler<ActiveShoppingListByStoreIdQuery, ShoppingListReadModel>
{
    private readonly Func<CancellationToken, IShoppingListQueryService> _shoppingListQueryServiceDelegate;

    public ActiveShoppingListByStoreIdQueryHandler(
        Func<CancellationToken, IShoppingListQueryService> shoppingListQueryServiceDelegate)
    {
        _shoppingListQueryServiceDelegate = shoppingListQueryServiceDelegate;
    }

    public async Task<ShoppingListReadModel> HandleAsync(ActiveShoppingListByStoreIdQuery query,
        CancellationToken cancellationToken)
    {
        var service = _shoppingListQueryServiceDelegate(cancellationToken);
        return await service.GetActiveAsync(query.StoreId);
    }
}