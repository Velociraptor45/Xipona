namespace Xipona.Frontend.Redux.Items.States.Merges;

public record MergeItemSelector(bool IsOpen, bool IsSearching,
    IReadOnlyCollection<MergeItemSearchResult> SearchResults, IReadOnlyCollection<MergeItemSearchResult> SelectedItems);