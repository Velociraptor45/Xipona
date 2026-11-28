using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor;
public record LoadItemForEditingAction(Guid ItemId);
public record LoadItemForEditingStartedAction;
public record LoadItemForEditingFinishedAction(EditedItem Item);