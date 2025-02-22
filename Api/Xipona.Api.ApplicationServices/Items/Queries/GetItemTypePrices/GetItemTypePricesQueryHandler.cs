using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.GetItemTypePrices;

public class GetItemTypePricesQueryHandler : IQueryHandler<GetItemTypePricesQuery, ItemTypePricesReadModel>
{
    private readonly Func<CancellationToken, IItemQueryService> _itemQueryServiceDelegate;

    public GetItemTypePricesQueryHandler(
        Func<CancellationToken, IItemQueryService> itemQueryServiceDelegate)
    {
        _itemQueryServiceDelegate = itemQueryServiceDelegate;
    }

    public Task<ItemTypePricesReadModel> HandleAsync(GetItemTypePricesQuery query, CancellationToken cancellationToken)
    {
        var service = _itemQueryServiceDelegate(cancellationToken);
        return service.GetItemTypePrices(query.ItemId, query.StoreId);
    }
}