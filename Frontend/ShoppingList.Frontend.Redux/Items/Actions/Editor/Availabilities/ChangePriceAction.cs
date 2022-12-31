using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangePriceAction(IAvailable Available, EditedItemAvailability Availability, float Price);