using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

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
    ProcessingErrors Errors)
{
    public IEnumerable<ShoppingListSection> GetSectionsToDisplay()
    {
        if (ShoppingList is null)
            return Enumerable.Empty<ShoppingListSection>();

        return ShoppingList.Sections.AsEnumerable()
            .Where(s => s.Items.Any() && (!s.AllItemsHidden || ItemsInBasketVisible));
    }

    public ShoppingListStore? SelectedStore => Stores.Stores.FirstOrDefault(s => s.Id == SelectedStoreId);
    public bool AllItemsInBasketHidden => !ItemsInBasketVisible && (ShoppingList?.AllItemsInBasket ?? false);
}

public class ShoppingListFeatureState : Feature<ShoppingListState>
{
    private readonly IWebAssemblyHostEnvironment _environment;

    public const float InitialTemporaryItemPrice = 1f;

    public ShoppingListFeatureState(IWebAssemblyHostEnvironment environment)
    {
        _environment = environment;
    }

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
            new SearchBar(string.Empty, new List<SearchItemForShoppingListResult>()),
            new TemporaryItemCreator(string.Empty, null, 1f, 0, false, false, false),
            new PriceUpdate(null, InitialTemporaryItemPrice, true, false, false),
            new Summary(false, false, DateTime.MinValue, false),
            new ProcessingErrors(!_environment.IsProduction(), false, new List<string>()));
    }
}