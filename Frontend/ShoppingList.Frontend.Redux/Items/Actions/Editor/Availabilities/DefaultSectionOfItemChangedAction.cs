using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record DefaultSectionOfItemChangedAction(EditedItemAvailability Availability, Guid DefaultSectionId);