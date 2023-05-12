namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

public record RecipeEditor(
    EditedRecipe? Recipe,
    string RecipeTagCreateInput,
    bool IsInEditMode);