using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

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
    Summary Summary,
    InitialStoreCreator InitialStoreCreator)
{
    public IEnumerable<ShoppingListSection> GetSectionsToDisplay()
    {
        if (ShoppingList is null)
            return [];

        return ShoppingList.Sections.AsEnumerable()
            .Where(s => s.Items.Any() && (!s.AllItemsHidden || ItemsInBasketVisible));
    }

    public ShoppingListStore? SelectedStore => Stores.Stores.FirstOrDefault(s => s.Id == SelectedStoreId);
    public bool AllItemsInBasketHidden => !ItemsInBasketVisible && (ShoppingList?.Sections.All(s => s.AllItemsHidden) ?? false);
}

public class ShoppingListFeatureState : Feature<ShoppingListState>
{
    public const decimal InitialTemporaryItemPrice = 1m;

    public override string GetName()
    {
        return nameof(ShoppingListState);
    }

    protected override ShoppingListState GetInitialState()
    {
        return new ShoppingListState(
            new List<QuantityType>(),
            new List<QuantityTypeInPacket>(),
            new AllActiveStores([]),
            Guid.Empty,
            true,
            false,
            null,
            new SearchBar(string.Empty, []),
            new TemporaryItemCreator(string.Empty, null, 1m, 0, false, false, false),
            new PriceUpdate(null, InitialTemporaryItemPrice, true, false, false, []),
            new Summary(false, false, DateTime.MinValue, false),
            new InitialStoreCreator(false, string.Empty, false));
    }
}