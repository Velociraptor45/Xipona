using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
public record ItemCategoryInputChangedAction(EditedIngredient Ingredient, string Input);