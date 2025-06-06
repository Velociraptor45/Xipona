using Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

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