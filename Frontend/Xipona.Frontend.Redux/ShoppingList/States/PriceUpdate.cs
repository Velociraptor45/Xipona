namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
public record PriceUpdate(ShoppingListItem? Item, decimal Price, bool UpdatePriceForAllTypes,
    bool IsOpen, bool IsSaving);