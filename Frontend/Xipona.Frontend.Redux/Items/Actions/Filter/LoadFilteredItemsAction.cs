using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Filter;

public record LoadFilteredItemsAction;

public record LoadFilteredItemsStartedAction;
public record LoadFilteredItemsFinishedAction(IReadOnlyList<ItemSearchResult> Items);