using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor;
public record ItemTypeNameChangedAction(EditedItemType ItemType, string? Name);