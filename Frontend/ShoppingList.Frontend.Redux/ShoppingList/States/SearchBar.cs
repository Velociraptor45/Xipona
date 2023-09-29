namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
public record SearchBar(string Input, IReadOnlyCollection<SearchItemForShoppingListResult> Results);