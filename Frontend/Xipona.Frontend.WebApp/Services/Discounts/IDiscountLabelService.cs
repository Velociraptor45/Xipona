using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.WebApp.Services.Discounts;

public interface IDiscountLabelService
{
    string GetLabel(ShoppingListDiscountType type);
}