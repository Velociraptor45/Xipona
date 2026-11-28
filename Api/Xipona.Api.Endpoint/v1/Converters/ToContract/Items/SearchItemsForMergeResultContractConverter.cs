using Xipona.Api.Contracts.Items.Queries.SearchItemsForMerge;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Items.Services.Searches;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

public class SearchItemsForMergeResultContractConverter : IToContractConverter<SearchItemsForMergeResult, SearchItemsForMergeResultContract>
{
    public SearchItemsForMergeResultContract ToContract(SearchItemsForMergeResult source)
    {
        return new SearchItemsForMergeResultContract(
            source.ItemId,
            source.ItemName,
            source.ItemCategoryId,
            source.ManufacturerId,
            source.ItemQuantity.Type.ToInt(),
            source.ItemQuantity.InPacket?.Quantity,
            source.ItemQuantity.InPacket?.Type.ToInt());
    }
}
