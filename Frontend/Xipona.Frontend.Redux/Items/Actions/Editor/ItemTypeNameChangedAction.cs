using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor;
public record ItemTypeNameChangedAction(EditedItemType ItemType, string? Name);