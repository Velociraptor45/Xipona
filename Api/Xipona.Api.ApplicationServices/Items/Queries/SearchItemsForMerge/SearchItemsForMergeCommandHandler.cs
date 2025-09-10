using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForMerge;

public record SearchItemsForMergeQuery(ItemCategoryId ItemCategoryId, ManufacturerId? ManufacturerId,
    ItemQuantity ItemQuantity, IReadOnlyCollection<ItemId> ExcludedItemIds) : IQuery<IEnumerable<SearchItemsForMergeResult>>;

public class SearchItemsForMergeQueryHandler : IQueryHandler<SearchItemsForMergeQuery, IEnumerable<SearchItemsForMergeResult>>
{
    private readonly Func<CancellationToken, IItemMergeSearchService> _serviceDelegate;

    public SearchItemsForMergeQueryHandler(Func<CancellationToken, IItemMergeSearchService> serviceDelegate)
    {
        _serviceDelegate = serviceDelegate;
    }

    public async Task<IEnumerable<SearchItemsForMergeResult>> HandleAsync(SearchItemsForMergeQuery query,
        CancellationToken cancellationToken)
    {
        var service = _serviceDelegate(cancellationToken);
        return await service.SearchAsync(query.ItemCategoryId, query.ManufacturerId, query.ItemQuantity,
            query.ExcludedItemIds);
    }
}
