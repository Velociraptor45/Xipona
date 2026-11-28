using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.Items.Queries.FilterItems;

public record FilterItemsQuery(StoreId? StoreId, ItemCategoryId? ItemCategoryId, ManufacturerId? ManufacturerId,
    int Page, int PageSize)
    : IQuery<IEnumerable<SearchItemResultReadModel>>;

public class FilterItemsQueryHandler : IQueryHandler<FilterItemsQuery, IEnumerable<SearchItemResultReadModel>>
{
    private readonly Func<CancellationToken, IItemSearchService> _itemQueryServiceDelegate;

    public FilterItemsQueryHandler(Func<CancellationToken, IItemSearchService> itemSearchServiceDelegate)
    {
        _itemQueryServiceDelegate = itemSearchServiceDelegate;
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> HandleAsync(
        FilterItemsQuery query, CancellationToken cancellationToken)
    {
        var itemSearchService = _itemQueryServiceDelegate(cancellationToken);
        return await itemSearchService.SearchAsync(query.StoreId, query.ItemCategoryId,
            query.ManufacturerId, query.Page, query.PageSize);
    }
}