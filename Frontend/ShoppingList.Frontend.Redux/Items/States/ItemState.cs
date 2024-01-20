using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
public record ItemState(
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
            new List<QuantityType>(),
            new List<QuantityTypeInPacket>(),
            new ActiveStores(new List<ItemStore>()),
            new ItemSearch(
                false,
                false,
                new List<ItemSearchResult>()),
            new ItemEditor(
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