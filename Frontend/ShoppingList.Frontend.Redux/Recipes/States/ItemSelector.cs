using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

public record ItemSelector(IReadOnlyCollection<SearchItemByItemCategoryResult> Items);