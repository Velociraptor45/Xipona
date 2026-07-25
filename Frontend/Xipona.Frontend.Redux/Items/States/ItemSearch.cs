namespace Xipona.Frontend.Redux.Items.States;
public record ItemSearch(
    ItemFilter Filter,
    string Input,
    int Page,
    int PageSize,
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IReadOnlyList<ItemSearchResult> SearchResults,
    int TotalResultCount);