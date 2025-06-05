using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Services.Queries;

namespace Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresOverview;

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