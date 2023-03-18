namespace ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;

public record ManufacturerEditor(
    EditedManufacturer? Manufacturer,
    bool IsLoadingEditedManufacturer,
    bool IsSaving,
    bool IsDeleting);