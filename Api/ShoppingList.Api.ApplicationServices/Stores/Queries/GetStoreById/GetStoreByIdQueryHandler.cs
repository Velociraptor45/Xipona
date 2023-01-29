using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.StoreById;

public class GetStoreByIdQueryHandler : IQueryHandler<GetStoreByIdQuery, IStore>
{
    private readonly Func<CancellationToken, IStoreQueryService> _storeQueryServiceDelegate;

    public GetStoreByIdQueryHandler(
        Func<CancellationToken, IStoreQueryService> storeQueryServiceDelegate)
    {
        _storeQueryServiceDelegate = storeQueryServiceDelegate;
    }

    public Task<IStore> HandleAsync(GetStoreByIdQuery query, CancellationToken cancellationToken)
    {
        var service = _storeQueryServiceDelegate(cancellationToken);
        return service.GetActiveAsync(query.StoreId);
    }
}