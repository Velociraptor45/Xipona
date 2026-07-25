using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Items.Actions.Filter;

public record SearchManufacturersAction;

public record SearchManufacturersFinishedAction(IReadOnlyList<ManufacturerSearchResult> Manufacturers);