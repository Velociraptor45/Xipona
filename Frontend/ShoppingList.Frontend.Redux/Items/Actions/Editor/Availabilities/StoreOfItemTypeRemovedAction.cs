using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record StoreOfItemTypeRemovedAction(EditedItemType ItemType, EditedItemAvailability Availability);