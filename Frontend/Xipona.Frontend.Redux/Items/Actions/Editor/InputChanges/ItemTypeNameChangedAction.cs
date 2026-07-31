using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Editor.InputChanges;
public record ItemTypeNameChangedAction(EditedItemType ItemType, string? Name);