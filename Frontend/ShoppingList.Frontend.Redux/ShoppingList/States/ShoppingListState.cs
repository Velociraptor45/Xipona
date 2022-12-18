using Fluxor;
using ShoppingList.Frontend.Redux.Shared.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.States;

public record ShoppingListState(
    IReadOnlyCollection<QuantityType> QuantityTypes,
    IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket,
    AllActiveStores Stores,
    Guid SelectedStoreId);

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
            Guid.Empty);
    }
}