using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions.Items;
public record ChangeItemQuantityFinishedAction(ShoppingListItemId ItemId, Guid? ItemTypeId, float NewQuantity);