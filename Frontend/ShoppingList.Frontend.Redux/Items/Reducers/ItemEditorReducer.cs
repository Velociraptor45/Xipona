using Fluxor;
using ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Reducers;

public static class ItemEditorReducer
{
    [ReducerMethod]
    public static ItemState OnQuantityTypeInPacketChanged(ItemState state, QuantityTypeInPacketChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    QuantityInPacketType = action.QuantityTypeInPacket
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnQuantityTypeChanged(ItemState state, QuantityTypeChangedAction action)
    {
        var newState = state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    QuantityType = action.QuantityType
                }
            }
        };

        if (action.QuantityType.Id == 1) // todo magic number #298
        {
            newState = newState with
            {
                Editor = newState.Editor with
                {
                    Item = newState.Editor.Item with
                    {
                        QuantityInPacket = null,
                        QuantityInPacketType = null
                    }
                }
            };
        }
        else if (newState.Editor.Item.QuantityInPacketType is null)
        {
            newState = newState with
            {
                Editor = newState.Editor with
                {
                    Item = newState.Editor.Item with
                    {
                        QuantityInPacket = 1,
                        QuantityInPacketType = newState.QuantityTypesInPacket.First()
                    }
                }
            };
        }

        return newState;
    }

    [ReducerMethod]
    public static ItemState OnItemQuantityInPacketChanged(ItemState state, ItemQuantityInPacketChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    QuantityInPacket = action.QuanityInPacket
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnItemCommentChanged(ItemState state, ItemCommentChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    Comment = action.Comment
                }
            }
        };
    }
}