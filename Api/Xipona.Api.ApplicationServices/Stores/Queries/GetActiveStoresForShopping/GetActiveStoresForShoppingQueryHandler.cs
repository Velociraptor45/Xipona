using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForShopping;

public class GetActiveStoresForShoppingQueryHandler : IQueryHandler<GetActiveStoresForShoppingQuery, IEnumerable<IStore>>
{
    private readonly Func<CancellationToken, IStoreQueryService> _storeQueryServiceDelegate;

    public GetActiveStoresForShoppingQueryHandler(Func<CancellationToken, IStoreQueryService> storeQueryServiceDelegate)
    {
        _storeQueryServiceDelegate = storeQueryServiceDelegate;
    }

    public async Task<IEnumerable<IStore>> HandleAsync(GetActiveStoresForShoppingQuery query,
        CancellationToken cancellationToken)
    {
        var service = _storeQueryServiceDelegate(cancellationToken);
        return await service.GetActiveAsync();
    }
}