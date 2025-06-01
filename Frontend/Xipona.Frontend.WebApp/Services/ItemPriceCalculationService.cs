using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Frontend.WebApp.Services;

public class ItemPriceCalculationService : IItemPriceCalculationService
{
    private readonly IState<ShoppingListState> _shoppingListState;

    public ItemPriceCalculationService(IState<ShoppingListState> shoppingListState)
    {
        _shoppingListState = shoppingListState;
    }

    public decimal CalculatePrice(int quantityTypeId, decimal pricePerQuantity, float quantity)
    {
        var type = _shoppingListState.Value.QuantityTypes.FirstOrDefault(type => type.Id == quantityTypeId);
        if (type == null)
            throw new InvalidOperationException($"Quantity type {quantityTypeId} not recognized.");

        var price = (decimal)quantity / type.QuantityNormalizer * pricePerQuantity;

        return Math.Round(price * 100, MidpointRounding.AwayFromZero) / 100;
    }

    private static decimal CalculatePrice(ShoppingListItem item)
    {
        var price = (decimal)item.Quantity / item.QuantityType.QuantityNormalizer * item.PricePerQuantity;

        return Math.Round(price * 100, MidpointRounding.AwayFromZero) / 100;
    }

    public decimal GetInBasketPrice(ShoppingListModel shoppingList)
    {
        var items = shoppingList.Items.Where(i => i.IsInBasket).ToList();
        var sum = 0m;
        foreach (var item in items)
        {
            sum += CalculatePrice(item);
        }
        return sum;
    }

    public decimal GetTotalPrice(ShoppingListModel shoppingList)
    {
        var items = shoppingList.Items.ToList();
        var sum = 0m;
        foreach (var item in items)
        {
            sum += CalculatePrice(item);
        }

        return SubtractDiscounts(sum, shoppingList.Discounts);
    }

    private decimal SubtractDiscounts(decimal totalListPrice, IReadOnlyCollection<ShoppingListDiscount> discounts)
    {
        var absolutePrices = discounts.Where(d => d.Price is not null).Sum(d => d.Price.Value);
        var percentages = discounts.Where(d => d.Percentage is not null).Sum(d => d.Percentage.Value);

        if (percentages > 100)
            percentages = 100;

        return (totalListPrice - absolutePrices) * (1 - percentages / 100);
    }
}