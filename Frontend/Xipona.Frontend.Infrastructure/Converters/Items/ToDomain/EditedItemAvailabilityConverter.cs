using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;

public class EditedItemAvailabilityConverter : IToDomainConverter<ItemAvailabilityContract, EditedItemAvailability>
{
    public EditedItemAvailability ToDomain(ItemAvailabilityContract source)
    {
        return new EditedItemAvailability(
            source.Store.Id,
            source.DefaultSection.Id,
            source.Price);
    }
}