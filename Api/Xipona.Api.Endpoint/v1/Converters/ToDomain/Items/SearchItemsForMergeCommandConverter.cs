using Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForMerge;
using Xipona.Api.Contracts.Items.Queries.SearchItemsForMerge;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

public class SearchItemsForMergeCommandConverter : IToDomainConverter<SearchItemsForMergeContract, SearchItemsForMergeCommand>
{
    public SearchItemsForMergeCommand ToDomain(SearchItemsForMergeContract source)
    {
        ItemQuantityInPacket? quantityTypeInPacket = null;
        if (source.Quantity is not null && source.QuantityTypeInPacket is not null)
            quantityTypeInPacket = new ItemQuantityInPacket(
                new Quantity(source.Quantity.Value),
                source.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());

        return new SearchItemsForMergeCommand(
            new ItemCategoryId(source.ItemCategory),
            source.Manufacturer is null ? null : new ManufacturerId(source.Manufacturer.Value),
            new ItemQuantity(
                source.QuantityType.ToEnum<QuantityType>(),
                quantityTypeInPacket),
            source.ExcludedItemIds.Select(i => new ItemId(i)).ToList());
    }
}
