namespace ProjectHermes.Xipona.Frontend.Redux.Items.States;
public record ItemSearch(
    string Input,
    int Page,
    int PageSize,
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IList<ItemSearchResult> SearchResults,
    int TotalResultCount);