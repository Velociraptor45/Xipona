namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

public record ItemEditor(
    EditedItem? Item,
    ItemCategorySelector ItemCategorySelector,
    ManufacturerSelector ManufacturerSelector,
    bool IsLoadingEditedItem,
    bool IsUpdating,
    bool IsModifying,
    bool IsDeleting,
    bool IsDeleteDialogOpen,
    EditorValidationResult ValidationResult);