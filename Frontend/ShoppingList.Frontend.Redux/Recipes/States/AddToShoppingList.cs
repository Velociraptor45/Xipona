namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

public record AddToShoppingList(
    int NumberOfServings,
    IReadOnlyCollection<AddToShoppingListIngredient> IngredientsForOneServing,
    IReadOnlyCollection<AddToShoppingListIngredient> Ingredients);