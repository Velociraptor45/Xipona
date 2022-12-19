using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ShoppingList.Frontend.Redux.ShoppingList.Actions;
public record PutItemInBasketAction(ShoppingListItemId ItemId, Guid? ItemTypeId);