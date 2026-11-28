namespace Xipona.Frontend.Redux.Items.States.Merges;

public record MergedItem(string Name, IReadOnlyCollection<MergedItemType> Types);
