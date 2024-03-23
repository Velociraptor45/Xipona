using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
public record SearchItemCategoriesFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories,
    Guid IngredientKey);