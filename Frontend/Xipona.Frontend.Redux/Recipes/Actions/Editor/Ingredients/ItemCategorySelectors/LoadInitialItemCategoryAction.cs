using Xipona.Frontend.Redux.ItemCategories.States;
using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
public record LoadInitialItemCategoryAction(EditedIngredient Ingredient);
public record LoadInitialItemCategoryFinishedAction(Guid IngredientKey, ItemCategorySearchResult Result);