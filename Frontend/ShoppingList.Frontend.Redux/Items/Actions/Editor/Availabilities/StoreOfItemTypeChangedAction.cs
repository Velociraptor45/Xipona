using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record StoreOfItemTypeChangedAction(EditedItemType ItemType, EditedItemAvailability Availability, Guid StoreId);