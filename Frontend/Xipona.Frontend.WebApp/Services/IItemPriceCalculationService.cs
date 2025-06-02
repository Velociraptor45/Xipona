using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.WebApp.Services;

public interface IItemPriceCalculationService
{
    decimal CalculatePrice(int quantityTypeId, decimal pricePerQuantity, float quantity);

    decimal GetInBasketPrice(ShoppingListModel shoppingList, bool includeDiscounts = false);

    decimal GetTotalPrice(ShoppingListModel shoppingList, bool includeDiscounts = false);
}