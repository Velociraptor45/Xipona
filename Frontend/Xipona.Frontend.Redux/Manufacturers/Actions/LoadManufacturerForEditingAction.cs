using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Redux.Manufacturers.Actions;

public record LoadManufacturerForEditingAction(Guid Id);
public record LoadManufacturerForEditingStartedAction;
public record LoadManufacturerForEditingFinishedAction(EditedManufacturer Manufacturer);
