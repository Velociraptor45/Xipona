namespace ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;

public record ManufacturerSearch(
    string Input,
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IList<ManufacturerSearchResult> SearchResults);