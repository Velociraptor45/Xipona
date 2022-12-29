using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.SearchBar;
public record SearchItemForShoppingListFinishedAction(IEnumerable<SearchItemForShoppingListResult> Results);