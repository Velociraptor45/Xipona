using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record DefaultSectionOfItemTypeChangedAction(EditedItemType ItemType, EditedItemAvailability Availability,
    Guid DefaultSectionId);