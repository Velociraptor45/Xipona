using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Items;
public record RemoveItemFromBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId, string ItemName);