using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangeDefaultSectionAction(IAvailable Available, EditedItemAvailability Availability,
    Guid DefaultSectionId);