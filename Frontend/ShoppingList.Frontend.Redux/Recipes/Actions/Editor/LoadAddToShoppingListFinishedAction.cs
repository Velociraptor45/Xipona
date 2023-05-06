using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
public record LoadAddToShoppingListFinishedAction(IReadOnlyCollection<AddToShoppingListIngredient> Ingredients);