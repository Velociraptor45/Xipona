using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions.Items;
public record RemoveItemFromBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId, string ItemName);