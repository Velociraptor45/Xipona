using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangePriceAction(IAvailable Available, EditedItemAvailability Availability, float Price);