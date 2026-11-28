using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Repositories.Items.Entities;

namespace Xipona.Api.Repositories.Items.Converters.ToDomain;

public class ItemTypeAvailabilityConverter : IToDomainConverter<ItemTypeAvailableAt, ItemAvailability>
{
    public ItemAvailability ToDomain(ItemTypeAvailableAt source)
    {
        return new ItemAvailability(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}