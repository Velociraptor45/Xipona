namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

public record AddToShoppingList(
    int NumberOfServings,
    IReadOnlyDictionary<(Guid, Guid?), float> ItemAmountsForOneServing,
    IReadOnlyCollection<AddToShoppingListItem> Items,
    bool IsSaving);