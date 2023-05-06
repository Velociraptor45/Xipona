namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;

public record AddIngredientToShoppingListChangedAction(Guid IngredientKey, bool AddToShoppingList);