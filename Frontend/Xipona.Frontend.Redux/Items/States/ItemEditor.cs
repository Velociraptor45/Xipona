﻿namespace ProjectHermes.Xipona.Frontend.Redux.Items.States;

public record ItemEditor(
    Guid? ItemId,
    EditedItem? Item,
    ItemCategorySelector ItemCategorySelector,
    ManufacturerSelector ManufacturerSelector,
    bool IsLoadingEditedItem,
    bool IsUpdating,
    bool IsModifying,
    bool IsDeleting,
    bool IsDeleteDialogOpen,
    EditorValidationResult ValidationResult);