using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
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
}