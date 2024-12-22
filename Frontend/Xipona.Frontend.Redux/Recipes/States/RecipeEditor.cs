namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

public record RecipeEditor(
    EditedRecipe? Recipe,
    string RecipeTagCreateInput,
    bool IsSaving,
    bool IsInEditMode,
    bool IsAddToShoppingListOpen,
    SideDishSelector SideDishSelector,
    AddToShoppingList? AddToShoppingList,
    EditorValidationResult ValidationResult);