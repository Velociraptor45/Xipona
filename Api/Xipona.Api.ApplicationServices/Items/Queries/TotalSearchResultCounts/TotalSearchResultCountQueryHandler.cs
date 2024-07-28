using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.TotalSearchResultCounts;
public class TotalSearchResultCountQueryHandler : IQueryHandler<TotalSearchResultCountQuery, int>
{
    private readonly Func<CancellationToken, IItemSearchService> _itemSearchServiceDelegate;

    public TotalSearchResultCountQueryHandler(Func<CancellationToken, IItemSearchService> itemSearchServiceDelegate)
    {
        _itemSearchServiceDelegate = itemSearchServiceDelegate;
    }

    public Task<int> HandleAsync(TotalSearchResultCountQuery query, CancellationToken cancellationToken)
    {
        var itemSearchService = _itemSearchServiceDelegate(cancellationToken);
        return itemSearchService.GetTotalSearchResultCountAsync(query.SearchInput);
    }
}
