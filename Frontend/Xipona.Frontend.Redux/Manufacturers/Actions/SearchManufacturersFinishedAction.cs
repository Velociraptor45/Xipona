using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Manufacturers.Actions;

public record SearchManufacturersFinishedAction(IReadOnlyCollection<ManufacturerSearchResult> SearchResults);