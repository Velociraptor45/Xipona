using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.WebApp.Services
{
    public interface IItemPriceCalculationService
    {
        float CalculatePrice(int quantityTypeId, decimal pricePerQuantity, float quantity);

        float GetInBasketPrice(ShoppingListModel shoppingList);

        float GetTotalPrice(ShoppingListModel shoppingList);
    }
}