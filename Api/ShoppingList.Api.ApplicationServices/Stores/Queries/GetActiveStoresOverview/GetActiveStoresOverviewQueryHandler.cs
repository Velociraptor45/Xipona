using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.GetActiveStoresOverview;

public class GetActiveStoresOverviewQueryHandler : IQueryHandler<GetActiveStoresOverviewQuery, IEnumerable<IStore>>
{
    private readonly Func<CancellationToken, IStoreQueryService> _storeQueryServiceDelegate;

    public GetActiveStoresOverviewQueryHandler(
        Func<CancellationToken, IStoreQueryService> storeQueryServiceDelegate)
    {
        _storeQueryServiceDelegate = storeQueryServiceDelegate;
    }

    public async Task<IEnumerable<IStore>> HandleAsync(GetActiveStoresOverviewQuery query,
        CancellationToken cancellationToken)
    {
        var service = _storeQueryServiceDelegate(cancellationToken);
        return await service.GetActiveAsync();
    }
}