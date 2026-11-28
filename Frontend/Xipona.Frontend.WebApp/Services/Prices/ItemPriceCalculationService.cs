using Fluxor;
using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.WebApp.Services.Prices;

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

    public decimal GetInBasketPrice(ShoppingListModel shoppingList, bool includeDiscounts = false)
    {
        var items = shoppingList.Items.Where(i => i.IsInBasket).ToList();
        var sum = 0m;
        foreach (var item in items)
        {
            sum += CalculatePrice(item);
        }

        if (!includeDiscounts)
            return sum;

        return SubtractDiscounts(sum, shoppingList.Discounts);
    }

    public decimal GetTotalPrice(ShoppingListModel shoppingList, bool includeDiscounts = false)
    {
        var items = shoppingList.Items.ToList();
        var sum = 0m;
        foreach (var item in items)
        {
            sum += CalculatePrice(item);
        }

        if (!includeDiscounts)
            return sum;

        return SubtractDiscounts(sum, shoppingList.Discounts);
    }

    private static decimal SubtractDiscounts(decimal totalListPrice, IReadOnlyCollection<ShoppingListDiscount> discounts)
    {
        var absolutePrices = discounts.Where(d => d.Type == ShoppingListDiscountType.Price).Sum(d => d.DiscountValue);
        var percentages = discounts.Where(d => d.Type == ShoppingListDiscountType.Percentage).Sum(d => d.DiscountValue);

        if (percentages > 100)
            percentages = 100;

        return (totalListPrice - absolutePrices) * (1 - percentages / 100);
    }
}