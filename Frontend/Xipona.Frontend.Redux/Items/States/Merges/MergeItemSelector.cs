namespace Xipona.Frontend.Redux.Items.States.Merges;

public record MergeItemSelector(bool IsOpen, string Input, IReadOnlyCollection<MergeItemSearchResult> SelectedItems);