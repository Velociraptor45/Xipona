using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.States;
public record ItemState(
    ItemStateInitialization Initialization,
    IReadOnlyCollection<QuantityType> QuantityTypes,
    IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket,
    ActiveStores Stores,
    ItemSearch Search,
    ItemEditor Editor);

public class ItemFeatureState : Feature<ItemState>
{
    public override string GetName()
    {
        return nameof(ItemState);
    }

    protected override ItemState GetInitialState()
    {
        return new ItemState(
            new ItemStateInitialization(false, false, false),
            new List<QuantityType>(),
            new List<QuantityTypeInPacket>(),
            new ActiveStores(new List<ItemStore>()),
            new ItemSearch(
                string.Empty,
                false,
                false,
                new List<ItemSearchResult>()),
            new ItemEditor(
                null,
                null,
                new ItemCategorySelector(new List<ItemCategorySearchResult>(), string.Empty),
                new ManufacturerSelector(new List<ManufacturerSearchResult>(), string.Empty),
                false,
                false,
                false,
                false,
                false,
                new()));
    }
}