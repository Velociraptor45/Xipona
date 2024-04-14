using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Items;
public record PutItemInBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId, string ItemName);