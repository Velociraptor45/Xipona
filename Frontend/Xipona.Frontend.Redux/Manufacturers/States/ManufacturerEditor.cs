namespace Xipona.Frontend.Redux.Manufacturers.States;

public record ManufacturerEditor(
    EditedManufacturer? ReadOnlyManufacturer,
    EditedManufacturer? Manufacturer,
    bool IsInEditMode,
    bool IsLoadingEditedManufacturer,
    bool IsSaving,
    bool IsDeleteDialogOpen,
    bool IsDeleting);