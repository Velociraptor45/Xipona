using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangeStoreAction(IAvailable Available, EditedItemAvailability Availability, Guid StoreId);