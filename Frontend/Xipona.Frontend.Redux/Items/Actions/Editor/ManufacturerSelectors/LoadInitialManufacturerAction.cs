using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
public record LoadInitialManufacturerAction;
public record LoadInitialManufacturerFinishedAction(ManufacturerSearchResult Manufacturer);