using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;
public record ChangeItemQuantityFinishedAction(ShoppingListItemId ItemId, Guid? ItemTypeId, float NewQuantity);