using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.SearchBar;
public record SearchItemForShoppingListFinishedAction(IReadOnlyCollection<SearchItemForShoppingListResult> Results);