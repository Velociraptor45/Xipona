using ShoppingList.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
public record SearchItemCategoriesFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories,
    Guid IngredientKey);