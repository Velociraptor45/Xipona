using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
public record LoadItemsForItemCategoryFinishedAction(IReadOnlyCollection<SearchItemByItemCategoryResult> Items,
    Guid IngredientId);