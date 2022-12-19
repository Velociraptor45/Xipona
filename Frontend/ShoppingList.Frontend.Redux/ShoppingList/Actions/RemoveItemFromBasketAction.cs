using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions;
public record RemoveItemFromBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId);