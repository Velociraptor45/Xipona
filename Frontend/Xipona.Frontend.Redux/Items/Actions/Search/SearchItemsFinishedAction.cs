using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Search;
public record SearchItemsFinishedAction(IReadOnlyCollection<ItemSearchResult> SearchResults);