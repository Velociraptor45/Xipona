using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;
public record ChangeItemQuantityFinishedAction(ShoppingListItemId ItemId, Guid? ItemTypeId, float NewQuantity);