using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ShoppingList.Frontend.Redux.Shared.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.States;

public record ShoppingListState(
    IReadOnlyCollection<QuantityType> QuantityTypes,
    IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket,
    AllActiveStores Stores,
    Guid SelectedStoreId,
    bool ItemsInBasketVisible,
    bool EditModeActive,
    ShoppingListModel? ShoppingList,
    SearchBar SearchBar,
    TemporaryItemCreator TemporaryItemCreator,
    PriceUpdate PriceUpdate,
    Summary Summary)
{
    public IEnumerable<ShoppingListSection> GetSectionsToDisplay()
    {
        return ShoppingList.Sections.AsEnumerable()
            .Where(s => s.Items.Any() && (!s.AllItemsHidden || ItemsInBasketVisible));
    }

    public ShoppingListStore? SelectedStore => Stores.Stores.FirstOrDefault(s => s.Id == SelectedStoreId);
}

public class ShoppingListFeatureState : Feature<ShoppingListState>
{
    public override string GetName()
    {
        return nameof(ShoppingListState);
    }

    protected override ShoppingListState GetInitialState()
    {
        return new ShoppingListState(
            new List<QuantityType>(),
            new List<QuantityTypeInPacket>(),
            new AllActiveStores(new List<ShoppingListStore>()),
            Guid.Empty,
            true,
            false,
            null,
            new SearchBar(string.Empty, false, new List<SearchItemForShoppingListResult>()),
            new TemporaryItemCreator(string.Empty, null, 1f, false, false, false),
            new PriceUpdate(null, 1f, true, false, false),
            new Summary(false, false, DateTime.MinValue, false));
    }
}