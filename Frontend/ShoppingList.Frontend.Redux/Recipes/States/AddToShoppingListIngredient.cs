namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
public record AddToShoppingListIngredient(
    Guid Key,
    Guid ItemId,
    Guid? ItemTypeId,
    int QuantityType,
    string QuantityLabel,
    float Quantity,
    Guid DefaultStoreId,
    bool AddToShoppingList,
    IReadOnlyCollection<AddToShoppingListAvailability> Availabilities);