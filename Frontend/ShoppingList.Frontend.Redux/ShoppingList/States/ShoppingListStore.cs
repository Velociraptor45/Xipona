namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

public record ShoppingListStore(Guid Id, string Name, IReadOnlyCollection<ShoppingListStoreSection> Sections);