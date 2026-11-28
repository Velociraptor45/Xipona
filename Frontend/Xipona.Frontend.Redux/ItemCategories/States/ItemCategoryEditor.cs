namespace Xipona.Frontend.Redux.ItemCategories.States;
public record ItemCategoryEditor(
    EditedItemCategory? ReadOnlyItemCategory,
    EditedItemCategory? ItemCategory,
    bool IsInEditMode,
    bool IsLoadingEditedItemCategory,
    bool IsSaving,
    bool IsDeleteDialogOpen,
    bool IsDeleting);