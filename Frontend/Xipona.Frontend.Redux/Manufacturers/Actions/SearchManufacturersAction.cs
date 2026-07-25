using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Manufacturers.Actions;

public record SearchManufacturersAction;
public record SearchManufacturersStartedAction;
public record SearchManufacturersFinishedAction(IReadOnlyCollection<ManufacturerSearchResult> SearchResults);