using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangeStoreAction(IAvailable Available, EditedItemAvailability Availability, Guid StoreId);