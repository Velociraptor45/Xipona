using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record DefaultSectionOfItemTypeChangedAction(EditedItemType ItemType, EditedItemAvailability Availability,
    Guid DefaultSectionId);