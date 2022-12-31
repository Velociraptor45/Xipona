using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangeDefaultSectionAction(IAvailable Available, EditedItemAvailability Availability,
    Guid DefaultSectionId);