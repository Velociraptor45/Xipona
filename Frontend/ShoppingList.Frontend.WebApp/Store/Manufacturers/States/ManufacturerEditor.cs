namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.States;

public record ManufacturerEditor(
    EditedManufacturer Manufacturer,
    bool IsLoadingEditedManufacturer,
    bool IsSaving,
    bool IsDeleting);