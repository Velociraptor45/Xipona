namespace ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;

public record ManufacturerEditor(
    EditedManufacturer? Manufacturer,
    bool IsLoadingEditedManufacturer,
    bool IsSaving,
    bool IsDeleteDialogOpen,
    bool IsDeleting);