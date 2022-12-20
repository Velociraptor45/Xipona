namespace ShoppingList.Frontend.Redux.ShoppingList.Actions;
public record SavePriceUpdateFinishedAction(Guid ItemId, Guid? ItemTypeId, float Price);