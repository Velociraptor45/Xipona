using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Items.Converters.ToDomain;

public class ItemAvailabilityConverter : IToDomainConverter<AvailableAt, ItemAvailability>
{
    public ItemAvailability ToDomain(AvailableAt source)
    {
        return new ItemAvailability(
            new StoreId(source.StoreId),
            new Price(source.Price),
            new SectionId(source.DefaultSectionId));
    }
}