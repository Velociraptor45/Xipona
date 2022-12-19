namespace ShoppingList.Frontend.Redux.ShoppingList.States;
public record TemporaryItemCreator(string ItemName, ShoppingListStoreSection? Section, float Price,
    bool IsButtonEnabled, bool IsOpen, bool IsSaving);