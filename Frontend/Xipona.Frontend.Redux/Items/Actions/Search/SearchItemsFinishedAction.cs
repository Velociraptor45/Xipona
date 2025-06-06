using Xipona.Frontend.Redux.Items.States;

namespace Xipona.Frontend.Redux.Items.Actions.Search;
public record SearchItemsFinishedAction(IReadOnlyCollection<ItemSearchResult> SearchResults);