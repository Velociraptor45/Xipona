namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record AddToShoppingListItem(
    Guid Key,
    Guid ItemId,
    string ItemName,
    Guid? ItemTypeId,
    int QuantityType,
    string QuantityLabel,
    float Quantity,
    Guid SelectedStoreId,
    bool AddToShoppingList,
    IReadOnlyCollection<AddToShoppingListAvailability> Availabilities);