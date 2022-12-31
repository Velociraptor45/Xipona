using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangeStoreAction(IAvailable Available, EditedItemAvailability Availability, Guid StoreId);