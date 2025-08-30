using Xipona.Frontend.Redux.Items.States.Merges;

namespace Xipona.Frontend.Redux.Items.Actions.Merges;

public record SearchItemsForMergeFinishedAction(IReadOnlyCollection<MergeItemSearchResult> SearchResults);
