using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
public record ChangePriceAction(IAvailable Available, EditedItemAvailability Availability, decimal Price);