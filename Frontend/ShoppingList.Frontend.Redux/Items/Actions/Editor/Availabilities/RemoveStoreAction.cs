using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record RemoveStoreAction(IAvailable Available, EditedItemAvailability Availability);