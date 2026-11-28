using Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Recipes.States;
using System;
using System.Linq;

namespace Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class AddToShoppingListIngredientConverter
    : IToDomainConverter<ItemAmountForOneServingContract, AddToShoppingListItem>
{
    private readonly IToDomainConverter<ItemAmountForOneServingAvailabilityContract, AddToShoppingListAvailability> _availabilityConverter;

    public AddToShoppingListIngredientConverter(
        IToDomainConverter<ItemAmountForOneServingAvailabilityContract, AddToShoppingListAvailability> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public AddToShoppingListItem ToDomain(ItemAmountForOneServingContract source)
    {
        return new AddToShoppingListItem(
            Guid.NewGuid(),
            source.ItemId,
            source.ItemName,
            source.ItemTypeId,
            source.QuantityType,
            source.QuantityLabel,
            source.Quantity,
            source.DefaultStoreId,
            source.AddToShoppingListByDefault,
            _availabilityConverter.ToDomain(source.Availabilities).ToList());
    }
}