using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Services.Queries;

namespace Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForItem;

public class GetActiveStoresForItemQueryHandler : IQueryHandler<GetActiveStoresForItemQuery, IEnumerable<IStore>>
{
    private readonly Func<CancellationToken, IStoreQueryService> _storeQueryServiceDelegate;

    public GetActiveStoresForItemQueryHandler(
        Func<CancellationToken, IStoreQueryService> storeQueryServiceDelegate)
    {
        _storeQueryServiceDelegate = storeQueryServiceDelegate;
    }

    public async Task<IEnumerable<IStore>> HandleAsync(GetActiveStoresForItemQuery query, CancellationToken cancellationToken)
    {
        var service = _storeQueryServiceDelegate(cancellationToken);
        return await service.GetActiveAsync();
    }
}