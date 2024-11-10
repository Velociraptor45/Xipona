namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
public record DiscountDialog(ShoppingListItem? Item, decimal Discount, bool IsOpen, bool IsSaving);