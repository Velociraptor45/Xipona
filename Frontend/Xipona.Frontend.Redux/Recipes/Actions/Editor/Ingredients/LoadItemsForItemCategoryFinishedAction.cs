using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
public record LoadItemsForItemCategoryFinishedAction(Guid IngredientKey,
    IReadOnlyCollection<SearchItemByItemCategoryResult> Items);