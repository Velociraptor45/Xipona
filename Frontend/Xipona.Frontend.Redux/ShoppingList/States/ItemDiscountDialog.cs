namespace Xipona.Frontend.Redux.ShoppingList.States;
public record ItemDiscountDialog(ShoppingListItem? Item, decimal Discount, bool IsOpen, bool IsSaving, bool IsRemoving);