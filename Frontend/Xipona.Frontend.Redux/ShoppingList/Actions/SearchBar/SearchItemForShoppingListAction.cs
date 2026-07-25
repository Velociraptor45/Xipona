using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions.SearchBar;
public record SearchItemForShoppingListAction;
public record SearchItemForShoppingListFinishedAction(IReadOnlyCollection<SearchItemForShoppingListResult> Results);