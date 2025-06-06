using Xipona.Api.Contracts.Items.Queries.Get;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;

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