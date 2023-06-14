using Fluxor;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;

public record ManufacturerState(
    ManufacturerSearch Search,
    ManufacturerEditor Editor);

public class ManufacturerFeatureState : Feature<ManufacturerState>
{
    public override string GetName()
    {
        return nameof(ManufacturerState);
    }

    protected override ManufacturerState GetInitialState()
    {
        return new ManufacturerState(
            new ManufacturerSearch(
                false,
                new List<ManufacturerSearchResult>()),
            new ManufacturerEditor(
                null,
                false,
                false,
                false));
    }
}