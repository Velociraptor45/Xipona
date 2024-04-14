using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Manufacturers.Actions;

public record SearchManufacturersFinishedAction(IReadOnlyCollection<ManufacturerSearchResult> SearchResults);