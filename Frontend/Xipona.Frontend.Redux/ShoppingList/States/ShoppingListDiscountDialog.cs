namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

public record ShoppingListDiscountDialog(decimal DiscountValue, ShoppingListDiscountType Type, bool IsOpen, bool IsSaving)
{
    public static ShoppingListDiscountDialog Default()
    {
        return new ShoppingListDiscountDialog(1m, ShoppingListDiscountType.Percentage, false, false);
    }
}