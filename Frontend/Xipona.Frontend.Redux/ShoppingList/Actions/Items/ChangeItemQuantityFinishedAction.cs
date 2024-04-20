using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Items;
public record ChangeItemQuantityFinishedAction(ShoppingListItemId ItemId, Guid? ItemTypeId, float NewQuantity);