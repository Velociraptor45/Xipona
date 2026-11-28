using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
public record SearchManufacturerAction;
public record SearchManufacturerFinishedAction(IReadOnlyCollection<ManufacturerSearchResult> SearchResults);