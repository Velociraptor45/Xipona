using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record PriceOfItemTypeChangedAction(EditedItemType ItemType, EditedItemAvailability Availability, float Price);