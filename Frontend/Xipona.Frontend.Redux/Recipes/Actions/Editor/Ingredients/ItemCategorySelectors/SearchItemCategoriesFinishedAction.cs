using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
public record SearchItemCategoriesFinishedAction(IReadOnlyCollection<ItemCategorySearchResult> ItemCategories,
    Guid IngredientKey);