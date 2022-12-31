using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record StoreOfItemChangedAction(EditedItemAvailability Availability, Guid StoreId);