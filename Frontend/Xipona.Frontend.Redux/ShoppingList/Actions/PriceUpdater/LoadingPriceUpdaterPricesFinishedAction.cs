using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;

public record LoadingPriceUpdaterPricesFinishedAction(IReadOnlyCollection<ItemTypePrice> Prices);