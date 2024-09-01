using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;

public record LoadingPriceUpdaterPricesFinishedAction(IReadOnlyCollection<ItemTypePrice> Prices);