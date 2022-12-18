﻿using Fluxor;
using ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListReducer
{
    [ReducerMethod]
    public static ShoppingListState OnLoadQuantityTypesFinished(ShoppingListState state,
        LoadQuantityTypesFinishedAction action)
    {
        return state with { QuantityTypes = action.QuantityTypes.ToList() };
    }

    [ReducerMethod]
    public static ShoppingListState OnLoadQuantityTypesInPacketFinished(ShoppingListState state,
        LoadQuantityTypesInPacketFinishedAction action)
    {
        return state with { QuantityTypesInPacket = action.QuantityTypesInPacket.ToList() };
    }
}