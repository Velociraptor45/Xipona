using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.States;

public record ManufacturerSearch(
    bool IsLoadingSearchResults,
    IList<ManufacturerSearchResult> SearchResults);