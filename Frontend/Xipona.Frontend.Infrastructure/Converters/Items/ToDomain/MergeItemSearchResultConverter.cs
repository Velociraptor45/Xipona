using Xipona.Api.Contracts.Items.Queries.SearchItemsForMerge;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Items.States.Merges;

namespace Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;
public class MergeItemSearchResultConverter : IToDomainConverter<SearchItemsForMergeResultContract, MergeItemSearchResult>
{
    public MergeItemSearchResult ToDomain(SearchItemsForMergeResultContract source)
    {
        return new MergeItemSearchResult(source.ItemId, source.Name, source.ItemCategoryId, source.ManufacturerId,
            source.QuantityType, source.QuantityTypeInPacket);
    }
}
