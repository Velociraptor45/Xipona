using Fluxor;
using ShoppingList.Frontend.Redux.Items.Actions;
using ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ShoppingList.Frontend.Redux.Items.States;

namespace ShoppingList.Frontend.Redux.Items.Reducers;

public static class ItemEditorReducer
{
    [ReducerMethod]
    public static ItemState OnItemNameChanged(ItemState state, ItemNameChangedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    Name = action.Name ?? string.Empty
                }
            }
        };
    }

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

    [ReducerMethod(typeof(SetNewItemAction))]
    public static ItemState OnSetNewItem(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = new EditedItem(
                    Guid.Empty,
                    "",
                    false,
                    "",
                    false,
                    state.QuantityTypes.First(),
                    null,
                    null,
                    null,
                    null,
                    new List<EditedItemAvailability>(),
                    new List<EditedItemType>())
            }
        };
    }

    [ReducerMethod(typeof(LoadItemForEditingStartedAction))]
    public static ItemState OnLoadItemForEditingStarted(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsLoadingEditedItem = true
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnLoadItemForEditingFinished(ItemState state, LoadItemForEditingFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsLoadingEditedItem = false,
                Item = action.Item
            }
        };
    }

    [ReducerMethod(typeof(StoreAddedToItemAction))]
    public static ItemState OnStoreAddedToItem(ItemState state)
    {
        var availabilities = state.Editor.Item!.Availabilities.ToList();
        var occupiedStoreIds = availabilities.Select(av => av.StoreId).ToHashSet();
        var availableStores = state.Stores.Stores.Where(s => !occupiedStoreIds.Contains(s.Id)).ToArray();

        if (!availableStores.Any())
            return state;

        var store = availableStores.First();
        availabilities.Add(new EditedItemAvailability(store.Id, store.DefaultSectionId, 1f));

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    Availabilities = availabilities
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnStoreAddedToItemType(ItemState state, StoreAddedToItemTypeAction action)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        var typeIndex = types.IndexOf(action.ItemType);

        if (typeIndex == -1)
            return state;

        var availabilities = action.ItemType.Availabilities.ToList();
        var occupiedStoreIds = availabilities.Select(av => av.StoreId).ToHashSet();
        var availableStores = state.Stores.Stores.Where(s => !occupiedStoreIds.Contains(s.Id)).ToArray();

        if (!availableStores.Any())
            return state;

        var store = availableStores.First();
        availabilities.Add(new EditedItemAvailability(store.Id, store.DefaultSectionId, 1f));

        for (int i = 0; i < availabilities.Count; i++)
        {
            availabilities[i] = availabilities[i] with { PricePerQuantity = 1f };
        }

        var itemType = action.ItemType with { Availabilities = availabilities };
        types[typeIndex] = itemType;

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnStoreOfItemChanged(ItemState state, StoreOfItemChangedAction action)
    {
        var availabilities = state.Editor.Item!.Availabilities.ToList();

        var availabilityIndex = availabilities.IndexOf(action.Availability);
        if (availabilityIndex == -1)
            return state;

        var store = state.Stores.Stores.FirstOrDefault(s => s.Id == action.StoreId);
        if (store == null)
            return state;

        availabilities[availabilityIndex] = new EditedItemAvailability(store.Id, store.DefaultSectionId, 1f);

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    Availabilities = availabilities
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnStoreOfItemTypeChanged(ItemState state, StoreOfItemTypeChangedAction action)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        var typeIndex = types.IndexOf(action.ItemType);

        if (typeIndex == -1)
            return state;

        var availabilities = action.ItemType.Availabilities.ToList();

        var availabilityIndex = availabilities.IndexOf(action.Availability);
        if (availabilityIndex == -1)
            return state;

        var store = state.Stores.Stores.FirstOrDefault(s => s.Id == action.StoreId);
        if (store == null)
            return state;

        availabilities[availabilityIndex] =
            new EditedItemAvailability(store.Id, store.DefaultSectionId, action.Availability.PricePerQuantity);

        var itemType = action.ItemType with { Availabilities = availabilities };
        types[typeIndex] = itemType;

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnDefaultSectionOfItemChanged(ItemState state, DefaultSectionOfItemChangedAction action)
    {
        var availabilities = state.Editor.Item!.Availabilities.ToList();

        var availabilityIndex = availabilities.IndexOf(action.Availability);
        if (availabilityIndex == -1)
            return state;

        availabilities[availabilityIndex] = action.Availability with { DefaultSectionId = action.DefaultSectionId };

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    Availabilities = availabilities
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnDefaultSectionOfItemTypeChanged(ItemState state, DefaultSectionOfItemTypeChangedAction action)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        var typeIndex = types.IndexOf(action.ItemType);

        if (typeIndex == -1)
            return state;

        var availabilities = action.ItemType.Availabilities.ToList();

        var availabilityIndex = availabilities.IndexOf(action.Availability);
        if (availabilityIndex == -1)
            return state;

        availabilities[availabilityIndex] = action.Availability with { DefaultSectionId = action.DefaultSectionId };

        var itemType = action.ItemType with { Availabilities = availabilities };
        types[typeIndex] = itemType;

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnPriceOfItemChanged(ItemState state, PriceOfItemChangedAction action)
    {
        var availabilities = state.Editor.Item!.Availabilities.ToList();

        var availabilityIndex = availabilities.IndexOf(action.Availability);
        if (availabilityIndex == -1)
            return state;

        availabilities[availabilityIndex] = action.Availability with { PricePerQuantity = action.Price };

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    Availabilities = availabilities
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnPriceOfItemTypeChanged(ItemState state, PriceOfItemTypeChangedAction action)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        var typeIndex = types.IndexOf(action.ItemType);

        if (typeIndex == -1)
            return state;

        var availabilities = action.ItemType.Availabilities.ToList();

        var availabilityIndex = availabilities.IndexOf(action.Availability);
        if (availabilityIndex == -1)
            return state;

        availabilities[availabilityIndex] = action.Availability with { PricePerQuantity = action.Price };

        var itemType = action.ItemType with { Availabilities = availabilities };
        types[typeIndex] = itemType;

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnStoreOfItemRemoved(ItemState state, StoreOfItemRemovedAction action)
    {
        var availabilities = state.Editor.Item!.Availabilities.ToList();
        availabilities.Remove(action.Availability);

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    Availabilities = availabilities
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnStoreOfItemTypeRemoved(ItemState state, StoreOfItemTypeRemovedAction action)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        var typeIndex = types.IndexOf(action.ItemType);

        if (typeIndex == -1)
            return state;

        var availabilities = action.ItemType.Availabilities.ToList();
        availabilities.Remove(action.Availability);

        types[typeIndex] = action.ItemType with { Availabilities = availabilities };

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnItemTypeNameChanged(ItemState state, ItemTypeNameChangedAction action)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        var typeIndex = types.IndexOf(action.ItemType);

        if (typeIndex == -1)
            return state;

        types[typeIndex] = action.ItemType with { Name = action.Name ?? string.Empty };

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                }
            }
        };
    }

    [ReducerMethod(typeof(ItemTypeAddedAction))]
    public static ItemState OnItemTypeAdded(ItemState state)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        types.Add(new(Guid.Empty, "", new List<EditedItemAvailability>()));

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnItemTypeRemoved(ItemState state, ItemTypeRemovedAction action)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        types.Remove(action.ItemType);

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                }
            }
        };
    }

    [ReducerMethod(typeof(EnterItemSearchPageAction))]
    public static ItemState OnEnterItemSearchPage(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Item = null
            }
        };
    }
}