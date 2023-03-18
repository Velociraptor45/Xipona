using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record DefaultSectionOfItemChangedAction(EditedItemAvailability Availability, Guid DefaultSectionId);