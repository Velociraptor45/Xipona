namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
public record TemporaryItemCreator(string ItemName, ShoppingListStoreSection? Section, float Price,
    int SelectedQuantityTypeId, bool IsButtonEnabled, bool IsOpen, bool IsSaving);