using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangeDefaultSectionAction(IAvailable Available, EditedItemAvailability Availability,
    Guid DefaultSectionId);