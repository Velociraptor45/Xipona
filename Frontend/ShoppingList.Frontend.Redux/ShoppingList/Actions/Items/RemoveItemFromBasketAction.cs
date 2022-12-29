using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions.Items;
public record RemoveItemFromBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId);