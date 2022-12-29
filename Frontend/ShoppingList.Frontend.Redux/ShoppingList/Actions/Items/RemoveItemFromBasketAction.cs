using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;
public record RemoveItemFromBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId);