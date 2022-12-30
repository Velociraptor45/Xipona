namespace ShoppingList.Frontend.Redux.Items.States;
public record ItemEditor(
    EditedItem? ItemCategory,
    bool IsLoadingEditedItem,
    bool IsSaving,
    bool IsDeleting);