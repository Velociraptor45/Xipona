namespace ShoppingList.Frontend.Redux.Manufacturers.States;

public record ManufacturerSearch(
    bool IsLoadingSearchResults,
    IList<ManufacturerSearchResult> SearchResults);