using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions.Items;
public record PutItemInBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId, string ItemName);