namespace ShoppingList.Frontend.Redux.ItemCategories.States;
public record ItemCategoryEditor(
    EditedItemCategory? ItemCategory,
    bool IsLoadingEditedItemCategory,
    bool IsSaving,
    bool IsDeleting);