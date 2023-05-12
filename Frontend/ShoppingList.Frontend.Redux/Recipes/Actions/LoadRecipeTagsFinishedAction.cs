using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions;
public record LoadRecipeTagsFinishedAction(IReadOnlyCollection<RecipeTag> RecipeTags);