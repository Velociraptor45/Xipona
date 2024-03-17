namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
public record SavePriceUpdateFinishedAction(Guid ItemId, Guid? ItemTypeId, float Price);