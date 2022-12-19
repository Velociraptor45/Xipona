using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions;
public record RemoveItemFromShoppingListAction(ShoppingListItemId ItemId, Guid? ItemTypeId);