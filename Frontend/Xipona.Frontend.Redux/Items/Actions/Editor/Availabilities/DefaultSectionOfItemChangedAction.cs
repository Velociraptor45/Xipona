using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record DefaultSectionOfItemChangedAction(EditedItemAvailability Availability, Guid DefaultSectionId);