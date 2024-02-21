using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States.Validators;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States.Validators;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Reducers;

public static class ItemEditorReducer
{
    private const int _weightQuantityTypeId = 1;

    private static readonly NameValidator _nameValidator = new();
    private static readonly ItemCategoryValidator _itemCategoryValidator = new();
    private static readonly StoresOrTypesValidator _storesOrTypesValidator = new();
    private static readonly TypesValidator _typesValidator = new();
    private static readonly StoresValidator _storesValidator = new();
    private static readonly TypeStoresValidator _typeStoresValidator = new();
    private static readonly DuplicatedStoresValidator _duplicatedStoresValidator = new();

    [ReducerMethod(typeof(ModifyItemAction))]
    public static ItemState OnModifyItem(ItemState state)
    {
        return OnSaveItem(state);
    }

    [ReducerMethod(typeof(UpdateItemAction))]
    public static ItemState OnUpdateItem(ItemState state)
    {
        return OnSaveItem(state);
    }

    [ReducerMethod(typeof(CreateItemAction))]
    public static ItemState OnCreateItem(ItemState state)
    {
        return OnSaveItem(state);
    }

    [ReducerMethod(typeof(MakeItemPermanentAction))]
    public static ItemState OnMakeItemPermanent(ItemState state)
    {
        return OnSaveItem(state);
    }

    private static ItemState OnSaveItem(ItemState state)
    {
        var item = state.Editor.Item;
        if (item == null)
            return state;

        _nameValidator.Validate(item.Name, out var nameError);
        _itemCategoryValidator.Validate(item.ItemCategoryId, out var itemCategoryError);
        _storesOrTypesValidator.Validate((item.ItemMode, item.Availabilities, item.ItemTypes), out var storesOrTypesError);
        _typesValidator.Validate((item.ItemMode, item.ItemTypes), out var typesError);
        _storesValidator.Validate((item.ItemMode, item.Availabilities), out var noStoresError);

        var typeNameResults = new Dictionary<Guid, string>();
        var noTypeStoreResults = new Dictionary<Guid, string>();
        var duplicatedTypeStoreResults = new Dictionary<Guid, string>();
        foreach (var itemType in item.ItemTypes)
        {
            _nameValidator.Validate(itemType.Name, out var typeNameError);
            _typeStoresValidator.Validate(itemType.Availabilities, out var typeStoresError);
            _duplicatedStoresValidator.Validate(itemType.Availabilities, out var duplicatedTypeStoresError);

            if (typeNameError is not null)
                typeNameResults.Add(itemType.Key, typeNameError);
            if (typeStoresError is not null)
                noTypeStoreResults.Add(itemType.Key, typeStoresError);
            if (duplicatedTypeStoresError is not null)
                duplicatedTypeStoreResults.Add(itemType.Key, duplicatedTypeStoresError);
        }

        _duplicatedStoresValidator.Validate(item.Availabilities, out var duplicatedStoresError);

        return state with
        {
            Editor = state.Editor with
            {
                ValidationResult = new EditorValidationResult(
                    nameError,
                    itemCategoryError,
                    storesOrTypesError,
                    noStoresError,
                    typesError,
                    typeNameResults,
                    noTypeStoreResults,
                    duplicatedTypeStoreResults,
                    duplicatedStoresError)
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnItemNameChanged(ItemState state, ItemNameChangedAction action)
    {
        if (state.Editor.Item is null)
            return state;

        _nameValidator.Validate(action.Name, out var nameError);

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item! with
                {
                    Name = action.Name ?? string.Empty
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    Name = nameError
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

        if (action.QuantityType.Id == _weightQuantityTypeId)
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
                    QuantityInPacket = action.QuantityInPacket
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
                    string.Empty,
                    false,
                    string.Empty,
                    false,
                    state.QuantityTypes.First(),
                    1,
                    state.QuantityTypesInPacket.First(),
                    null,
                    null,
                    new List<EditedItemAvailability>(),
                    new List<EditedItemType>(),
                    ItemMode.NotDefined),
                ItemCategorySelector = state.Editor.ItemCategorySelector with
                {
                    ItemCategories = new List<ItemCategorySearchResult>(0)
                },
                ManufacturerSelector = state.Editor.ManufacturerSelector with
                {
                    Manufacturers = new List<ManufacturerSearchResult>(0)
                },
                ValidationResult = new()
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
                Item = action.Item,
                ValidationResult = new()
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
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    NoStores = null,
                    StoreOrTypes = null
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnStoreAddedToItemType(ItemState state, StoreAddedToItemTypeAction action)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        var type = types.FirstOrDefault(t => t.Key == action.ItemTypeKey);
        if (type == null)
            return state;

        var typeIndex = types.IndexOf(type);

        var availabilities = type.Availabilities.ToList();
        var occupiedStoreIds = availabilities.Select(av => av.StoreId).ToHashSet();
        var availableStores = state.Stores.Stores.Where(s => !occupiedStoreIds.Contains(s.Id)).ToArray();

        if (!availableStores.Any())
            return state;

        var store = availableStores.First();
        availabilities.Add(new EditedItemAvailability(store.Id, store.DefaultSectionId, 1f));

        var itemType = type with { Availabilities = availabilities };
        types[typeIndex] = itemType;

        var noTypeStores = state.Editor.ValidationResult.NoTypeStores.ToDictionary(x => x.Key, x => x.Value);
        noTypeStores.Remove(type.Key);

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    NoTypeStores = noTypeStores
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnStoreOfItemChanged(ItemState state, StoreOfItemChangedAction action)
    {
        if (action.StoreId == action.Availability.StoreId)
            return state;

        var availabilities = state.Editor.Item!.Availabilities.ToList();

        var availabilityIndex = availabilities.IndexOf(action.Availability);
        if (availabilityIndex == -1)
            return state;

        var store = state.Stores.Stores.FirstOrDefault(s => s.Id == action.StoreId);
        if (store == null)
            return state;

        availabilities[availabilityIndex] =
            new EditedItemAvailability(store.Id, store.DefaultSectionId, action.Availability.PricePerQuantity);

        _duplicatedStoresValidator.Validate(availabilities, out var duplicatedStoresError);

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    Availabilities = availabilities
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    DuplicatedStores = duplicatedStoresError
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnStoreOfItemTypeChanged(ItemState state, StoreOfItemTypeChangedAction action)
    {
        if (action.StoreId == action.Availability.StoreId)
            return state;

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

        var duplicatedTypeStoresError = state.Editor.ValidationResult.DuplicatedTypeStores
            .ToDictionary(x => x.Key, x => x.Value);

        _duplicatedStoresValidator.Validate(availabilities, out var duplicatedStoresError);
        if (duplicatedStoresError is null)
            duplicatedTypeStoresError.Remove(action.ItemType.Key);
        else
            duplicatedTypeStoresError[action.ItemType.Key] = duplicatedStoresError;

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    DuplicatedTypeStores = duplicatedTypeStoresError
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
        if (!availabilities.Remove(action.Availability))
            return state;

        _duplicatedStoresValidator.Validate(availabilities, out var duplicatedStoresError);

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    Availabilities = availabilities
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    DuplicatedStores = duplicatedStoresError
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

        var duplicatedStoresErrors = state.Editor.ValidationResult.DuplicatedTypeStores
            .ToDictionary(x => x.Key, x => x.Value);
        if (_duplicatedStoresValidator.Validate(availabilities, out var duplicatedStoresError))
            duplicatedStoresErrors.Remove(action.ItemType.Key);
        else
            duplicatedStoresErrors[action.ItemType.Key] = duplicatedStoresError!;

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    DuplicatedTypeStores = duplicatedStoresErrors
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

        var modifiedType = action.ItemType with { Name = action.Name ?? string.Empty };
        types[typeIndex] = modifiedType;

        var typeNameResults = state.Editor.ValidationResult.TypeNames.ToDictionary(x => x.Key, x => x.Value);

        if (_nameValidator.Validate(modifiedType.Name, out var nameErrorMessage))
            typeNameResults.Remove(modifiedType.Key);
        else
            typeNameResults[modifiedType.Key] = nameErrorMessage!;

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    TypeNames = typeNameResults
                }
            }
        };
    }

    [ReducerMethod(typeof(ItemTypeAddedAction))]
    public static ItemState OnItemTypeAdded(ItemState state)
    {
        var types = state.Editor.Item!.ItemTypes.ToList();
        types.Insert(0, new(Guid.Empty, Guid.NewGuid(), string.Empty, new List<EditedItemAvailability>()));

        return state with
        {
            Editor = state.Editor with
            {
                Item = state.Editor.Item with
                {
                    ItemTypes = types
                },
                ValidationResult = state.Editor.ValidationResult with
                {
                    NoTypes = null,
                    StoreOrTypes = null
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

    [ReducerMethod(typeof(CreateItemStartedAction))]
    public static ItemState OnCreateItemStarted(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsModifying = true
            }
        };
    }

    [ReducerMethod(typeof(CreateItemFinishedAction))]
    public static ItemState OnCreateItemFinished(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsModifying = false
            }
        };
    }

    [ReducerMethod(typeof(UpdateItemStartedAction))]
    public static ItemState OnUpdateItemStarted(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsUpdating = true
            }
        };
    }

    [ReducerMethod(typeof(UpdateItemFinishedAction))]
    public static ItemState OnUpdateItemFinished(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsUpdating = false
            }
        };
    }

    [ReducerMethod(typeof(ModifyItemStartedAction))]
    public static ItemState OnModifyItemStarted(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsModifying = true
            }
        };
    }

    [ReducerMethod(typeof(ModifyItemFinishedAction))]
    public static ItemState OnModifyItemFinished(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsModifying = false
            }
        };
    }

    [ReducerMethod(typeof(MakeItemPermanentStartedAction))]
    public static ItemState OnMakeItemPermanentStarted(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsModifying = true
            }
        };
    }

    [ReducerMethod(typeof(MakeItemPermanentFinishedAction))]
    public static ItemState OnMakeItemPermanentFinished(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsModifying = false
            }
        };
    }

    [ReducerMethod(typeof(DeleteItemStartedAction))]
    public static ItemState OnDeleteItemStarted(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleting = true
            }
        };
    }

    [ReducerMethod(typeof(DeleteItemFinishedAction))]
    public static ItemState OnDeleteItemFinished(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleting = false
            }
        };
    }

    [ReducerMethod(typeof(OpenDeleteItemDialogAction))]
    public static ItemState OnOpenDeleteItemDialog(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleteDialogOpen = true
            }
        };
    }

    [ReducerMethod(typeof(CloseDeleteItemDialogAction))]
    public static ItemState OnCloseDeleteItemDialog(ItemState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleteDialogOpen = false
            }
        };
    }
}