using Xipona.Frontend.Redux.Recipes.States;

namespace Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
public record LoadItemsForItemCategoryAction(Guid IngredientKey, Guid ItemCategoryId);
public record LoadItemsForItemCategoryFinishedAction(Guid IngredientKey,
    IReadOnlyCollection<SearchItemByItemCategoryResult> Items);