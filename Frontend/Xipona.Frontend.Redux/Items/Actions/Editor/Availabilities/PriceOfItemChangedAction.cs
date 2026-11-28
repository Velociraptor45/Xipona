using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;

public record PriceOfItemChangedAction(EditedItemAvailability Availability, decimal Price);