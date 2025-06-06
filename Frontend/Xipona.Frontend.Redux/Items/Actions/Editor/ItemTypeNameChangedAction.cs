using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor;
public record ItemTypeNameChangedAction(EditedItemType ItemType, string? Name);