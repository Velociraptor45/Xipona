using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record DefaultSectionOfItemTypeChangedAction(EditedItemType ItemType, EditedItemAvailability Availability,
    Guid DefaultSectionId);