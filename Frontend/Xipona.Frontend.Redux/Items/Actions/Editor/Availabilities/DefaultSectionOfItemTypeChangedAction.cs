using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record DefaultSectionOfItemTypeChangedAction(EditedItemType ItemType, EditedItemAvailability Availability,
    Guid DefaultSectionId);