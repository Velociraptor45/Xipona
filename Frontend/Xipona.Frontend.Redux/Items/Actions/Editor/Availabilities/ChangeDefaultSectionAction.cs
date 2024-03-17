using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangeDefaultSectionAction(IAvailable Available, EditedItemAvailability Availability,
    Guid DefaultSectionId);