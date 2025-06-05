using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Items.Services.Searches;

namespace Xipona.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;

public class SearchItemsByItemCategoryQueryHandler :
    IQueryHandler<SearchItemsByItemCategoryQuery, IEnumerable<SearchItemByItemCategoryResult>>
{
    private readonly Func<CancellationToken, IItemSearchService> _itemSearchServiceDelegate;

    public SearchItemsByItemCategoryQueryHandler(
        Func<CancellationToken, IItemSearchService> itemSearchServiceDelegate)
    {
        _itemSearchServiceDelegate = itemSearchServiceDelegate;
    }

    public async Task<IEnumerable<SearchItemByItemCategoryResult>> HandleAsync(SearchItemsByItemCategoryQuery query,
        CancellationToken cancellationToken)
    {
        var service = _itemSearchServiceDelegate(cancellationToken);
        return await service.SearchAsync(query.ItemCategoryId);
    }
}