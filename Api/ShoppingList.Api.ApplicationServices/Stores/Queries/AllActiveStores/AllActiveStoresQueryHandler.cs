using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.AllActiveStores;

public class AllActiveStoresQueryHandler : IQueryHandler<AllActiveStoresQuery, IEnumerable<IStore>>
{
    private readonly Func<CancellationToken, IStoreQueryService> _storeQueryServiceDelegate;

    public AllActiveStoresQueryHandler(Func<CancellationToken, IStoreQueryService> storeQueryServiceDelegate)
    {
        _storeQueryServiceDelegate = storeQueryServiceDelegate;
    }

    public async Task<IEnumerable<IStore>> HandleAsync(AllActiveStoresQuery query,
        CancellationToken cancellationToken)
    {
        var service = _storeQueryServiceDelegate(cancellationToken);
        return await service.GetActiveAsync();
    }
}