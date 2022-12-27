using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Services
{
    public interface IItemPriceCalculationService
    {
        float CalculatePrice(int quantityTypeId, float pricePerQuantity, float quantity);

        float GetInBasketPrice(ShoppingListModel shoppingList);

        float GetTotalPrice(ShoppingListModel shoppingList);
    }
}