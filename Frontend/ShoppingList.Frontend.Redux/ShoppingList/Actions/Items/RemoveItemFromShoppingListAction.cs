using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;
public record RemoveItemFromShoppingListAction(ShoppingListItemId ItemId, Guid? ItemTypeId);