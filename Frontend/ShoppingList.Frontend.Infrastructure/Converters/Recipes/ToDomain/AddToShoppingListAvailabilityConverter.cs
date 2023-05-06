using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

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