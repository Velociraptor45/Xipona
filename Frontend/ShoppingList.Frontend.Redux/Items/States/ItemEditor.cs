using ShoppingList.Frontend.Redux.Manufacturers.States;

namespace ShoppingList.Frontend.Redux.Items.States;

public record ItemEditor(
    EditedItem? Item,
    ItemCategorySelector ItemCategorySelector,
    IReadOnlyCollection<ManufacturerSearchResult> Manufacturers,
    bool IsLoadingEditedItem,
    bool IsSaving,
    bool IsDeleting)
{
    public bool DisableSaveButtons => Item is not null
                                      && Item.IsItemWithTypes
                                      && !Item.ItemTypes.Any();
};