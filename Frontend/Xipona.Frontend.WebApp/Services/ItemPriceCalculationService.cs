using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using System;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.WebApp.Services;

public class ItemPriceCalculationService : IItemPriceCalculationService
{
    private readonly IState<ShoppingListState> _shoppingListState;

    public ItemPriceCalculationService(IState<ShoppingListState> shoppingListState)
    {
        _shoppingListState = shoppingListState;
    }

    public float CalculatePrice(int quantityTypeId, decimal pricePerQuantity, float quantity)
    {
        var type = _shoppingListState.Value.QuantityTypes.FirstOrDefault(type => type.Id == quantityTypeId);
        if (type == null)
            throw new InvalidOperationException($"Quantity type {quantityTypeId} not recognized.");

        var price = (decimal)quantity / type.QuantityNormalizer * pricePerQuantity;

        return (float)Math.Round(price * 100, MidpointRounding.AwayFromZero) / 100;
    }

    private static float CalculatePrice(ShoppingListItem item)
    {
        var price = (decimal)item.Quantity / item.QuantityType.QuantityNormalizer * item.PricePerQuantity;

        return (float)Math.Round(price * 100, MidpointRounding.AwayFromZero) / 100;
    }

    public float GetInBasketPrice(ShoppingListModel shoppingList)
    {
        var items = shoppingList.Items.Where(i => i.IsInBasket).ToList();
        var sum = 0f;
        foreach (var item in items)
        {
            sum += CalculatePrice(item);
        }
        return sum;
    }

    public float GetTotalPrice(ShoppingListModel shoppingList)
    {
        var items = shoppingList.Items.ToList();
        var sum = 0f;
        foreach (var item in items)
        {
            sum += CalculatePrice(item);
        }
        return sum;
    }
}