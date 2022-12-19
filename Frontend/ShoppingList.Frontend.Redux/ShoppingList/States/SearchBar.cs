using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ShoppingList.Frontend.Redux.ShoppingList.States;
public record SearchBar(string Input, bool IsActive, IReadOnlyCollection<SearchItemForShoppingListResult> Results);