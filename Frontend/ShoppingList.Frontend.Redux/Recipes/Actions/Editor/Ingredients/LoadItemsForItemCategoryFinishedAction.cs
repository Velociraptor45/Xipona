using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
public record LoadItemsForItemCategoryFinishedAction(IReadOnlyCollection<SearchItemByItemCategoryResult> Items,
    Guid IngredientKey);