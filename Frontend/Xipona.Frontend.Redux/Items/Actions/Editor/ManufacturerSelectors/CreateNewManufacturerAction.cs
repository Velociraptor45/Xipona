using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
public record CreateNewManufacturerAction;
public record CreateNewManufacturerFinishedAction(ManufacturerSearchResult Manufacturer);