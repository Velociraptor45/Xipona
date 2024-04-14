using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class AddToShoppingListAvailabilityConverter
    : IToDomainConverter<ItemAmountForOneServingAvailabilityContract, AddToShoppingListAvailability>
{
    public AddToShoppingListAvailability ToDomain(ItemAmountForOneServingAvailabilityContract source)
    {
        return new AddToShoppingListAvailability(
            source.StoreId,
            source.StoreName,
            source.Price);
    }
}