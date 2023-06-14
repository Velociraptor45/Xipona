using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
public record LoadItemsForItemCategoryFinishedAction(Guid IngredientKey,
    IReadOnlyCollection<SearchItemByItemCategoryResult> Items);