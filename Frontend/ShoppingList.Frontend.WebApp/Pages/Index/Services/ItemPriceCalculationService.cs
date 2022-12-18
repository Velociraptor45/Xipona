using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services;
using ShoppingList.Frontend.Redux.ShoppingList.States;
using System;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services;

public class ItemPriceCalculationService : IItemPriceCalculationService
{
    private readonly IState<ShoppingListState> _shoppingListState;

    public ItemPriceCalculationService(IState<ShoppingListState> shoppingListState)
    {
        _shoppingListState = shoppingListState;
    }

    public float CalculatePrice(int quantityTypeId, float pricePerQuantity, float quantity)
    {
        var type = _shoppingListState.Value.QuantityTypes.FirstOrDefault(type => type.Id == quantityTypeId);
        if (type == null)
            throw new InvalidOperationException($"Quantity type {quantityTypeId} not recognized.");

        float price = (quantity / type.QuantityNormalizer) * pricePerQuantity;

        return (float)Math.Round(price * 100, MidpointRounding.AwayFromZero) / 100;
    }
}