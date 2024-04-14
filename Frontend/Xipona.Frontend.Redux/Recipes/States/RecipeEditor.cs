namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

public record RecipeEditor(
    EditedRecipe? Recipe,
    string RecipeTagCreateInput,
    bool IsSaving,
    bool IsInEditMode,
    bool IsAddToShoppingListOpen,
    AddToShoppingList? AddToShoppingList,
    EditorValidationResult ValidationResult);