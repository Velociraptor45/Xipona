using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangePriceAction(IAvailable Available, EditedItemAvailability Availability, decimal Price);