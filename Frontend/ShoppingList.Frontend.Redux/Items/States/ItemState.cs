﻿using Fluxor;
using ShoppingList.Frontend.Redux.Shared.States;

namespace ShoppingList.Frontend.Redux.Items.States;
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
                new List<ItemSearchResult>()),
            new ItemEditor(
                null,
                false,
                false,
                false));
    }
}