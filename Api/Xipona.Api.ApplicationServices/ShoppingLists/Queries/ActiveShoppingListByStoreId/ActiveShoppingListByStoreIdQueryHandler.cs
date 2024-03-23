using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Queries.ActiveShoppingListByStoreId;

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