using Xipona.Api.Contracts.Items.Commands.Shared;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

public class ItemAvailabilityConverter : IToDomainConverter<ItemAvailabilityContract, ItemAvailability>
{
    public ItemAvailability ToDomain(ItemAvailabilityContract source)
    {
        return new ItemAvailability(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}