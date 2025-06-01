using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.WebApp.Services.Discounts;

public interface IDiscountLabelService
{
    string GetLabel(ShoppingListDiscountType type);
}