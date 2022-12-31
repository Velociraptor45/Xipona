using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor;
public record ItemTypeNameChangedAction(EditedItemType ItemType, string? Name);