using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
public record SearchManufacturerFinishedAction(IReadOnlyCollection<ManufacturerSearchResult> SearchResults);