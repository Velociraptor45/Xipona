using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;
public record PutItemInBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId);