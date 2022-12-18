using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Store.Manufacturers.States;

public record ManufacturerState(
    bool IsLoadingSearchResults,
    IList<ManufacturerSearchResult> SearchResults);

public class ManufacturerFeatureState : Feature<ManufacturerState>
{
    public override string GetName()
    {
        return nameof(ManufacturerState);
    }

    protected override ManufacturerState GetInitialState()
    {
        return new ManufacturerState(false, new List<ManufacturerSearchResult>());
    }
}