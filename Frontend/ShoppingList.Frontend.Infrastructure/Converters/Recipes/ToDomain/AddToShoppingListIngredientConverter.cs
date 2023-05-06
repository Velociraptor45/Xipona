﻿using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class AddToShoppingListIngredientConverter
    : IToDomainConverter<ItemAmountForOneServingContract, AddToShoppingListIngredient>
{
    private readonly IToDomainConverter<ItemAmountForOneServingAvailabilityContract, AddToShoppingListAvailability> _availabilityConverter;

    public AddToShoppingListIngredientConverter(
        IToDomainConverter<ItemAmountForOneServingAvailabilityContract, AddToShoppingListAvailability> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public AddToShoppingListIngredient ToDomain(ItemAmountForOneServingContract source)
    {
        return new AddToShoppingListIngredient(
            Guid.NewGuid(),
            source.ItemId,
            source.ItemTypeId,
            source.QuantityType,
            source.QuantityLabel,
            source.Quantity,
            source.DefaultStoreId,
            source.AddToShoppingListByDefault,
            _availabilityConverter.ToDomain(source.Availabilities).ToList());
    }
}