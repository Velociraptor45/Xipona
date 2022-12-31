namespace ShoppingList.Frontend.Redux.Items.States;

public record ItemEditor(
    EditedItem? Item,
    bool IsLoadingEditedItem,
    bool IsSaving,
    bool IsDeleting)
{
    public bool DisableSaveButtons => Item is not null
                                      && Item.IsItemWithTypes
                                      && !Item.ItemTypes.Any();
};