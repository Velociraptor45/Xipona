namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

public record ItemEditor(
    EditedItem? Item,
    ItemCategorySelector ItemCategorySelector,
    ManufacturerSelector ManufacturerSelector,
    bool IsLoadingEditedItem,
    bool IsUpdating,
    bool IsModifying,
    bool IsDeleting)
{
    public bool DisableSaveButtons => Item is not null
                                      && Item.IsItemWithTypes
                                      && !Item.ItemTypes.Any();
};