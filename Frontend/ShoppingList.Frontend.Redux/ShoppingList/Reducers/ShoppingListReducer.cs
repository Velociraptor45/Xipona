using Fluxor;
using ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ShoppingList.Frontend.Redux.ShoppingList.States;
using ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;
using OldModels = ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

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

    [ReducerMethod]
    public static ShoppingListState OnLoadAllActiveStoresFinished(ShoppingListState state,
        LoadAllActiveStoresFinishedAction action)
    {
        return state with { Stores = action.Stores };
    }

    [ReducerMethod]
    public static ShoppingListState OnSelectedStoreChanged(ShoppingListState state,
        SelectedStoreChangedAction action)
    {
        return state with { SelectedStoreId = action.StoreId };
    }

    [ReducerMethod]
    public static ShoppingListState OnLoadShoppingListFinished(ShoppingListState state,
        LoadShoppingListFinishedAction action)
    {
        return state with
        {
            EditModeActive = false,
            ItemsInBasketVisible = true,
            ShoppingList = action.ShoppingList
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnToggleItemsInBasketVisible(ShoppingListState state,
        ToggleItemsInBasketVisibleAction action)
    {
        return state with { ItemsInBasketVisible = !state.ItemsInBasketVisible };
    }

    [ReducerMethod]
    public static ShoppingListState OnToggleEditMode(ShoppingListState state,
        ToggleEditModeAction action)
    {
        return state with { EditModeActive = !state.EditModeActive };
    }

    [ReducerMethod]
    public static ShoppingListState OnItemForShoppingListSearchInputChanged(ShoppingListState state,
        ItemForShoppingListSearchInputChangedAction action)
    {
        var input = action.Input.Trim();
        return state with
        {
            SearchBar = state.SearchBar with
            {
                Input = input,
                Results = input == string.Empty
                    ? new List<OldModels.SearchItemForShoppingListResult>()
                    : state.SearchBar.Results
            },
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                IsButtonEnabled = !string.IsNullOrEmpty(input)
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSearchItemForShoppingListFinished(ShoppingListState state,
        SearchItemForShoppingListFinishedAction action)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                Results = action.Results.ToList()
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnItemForShoppingListSearchResultSelected(ShoppingListState state,
        ItemForShoppingListSearchResultSelectedAction action)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                Input = string.Empty,
                Results = new List<OldModels.SearchItemForShoppingListResult>()
            },
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                ItemName = string.Empty
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnTemporaryItemNameChanged(ShoppingListState state,
        TemporaryItemNameChangedAction action)
    {
        return state with
        {
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                ItemName = action.ItemName
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnTemporaryItemPriceChanged(ShoppingListState state,
        TemporaryItemPriceChangedAction action)
    {
        return state with
        {
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                Price = action.Price
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnTemporaryItemSelectedSectionChanged(ShoppingListState state,
        TemporaryItemSelectedSectionChangedAction action)
    {
        return state with
        {
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                Section = action.Section
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnOpenTemporaryItemCreator(ShoppingListState state,
        OpenTemporaryItemCreatorAction action)
    {
        return state with
        {
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                ItemName = state.SearchBar.Input,
                IsOpen = true,
                IsSaving = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnCloseTemporaryItemCreator(ShoppingListState state,
        CloseTemporaryItemCreatorAction action)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                Input = string.Empty
            },
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                ItemName = string.Empty,
                IsOpen = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSaveTemporaryItemStarted(ShoppingListState state,
        SaveTemporaryItemStartedAction action)
    {
        return state with
        {
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSaveTemporaryItemFinished(ShoppingListState state,
        SaveTemporaryItemFinishedAction action)
    {
        var sections = state.ShoppingList!.Sections.ToList();
        var section = sections.FirstOrDefault(s => s.Id == action.Section.Id);
        if (section is null)
        {
            section = new ShoppingListSection(
                action.Section.Id,
                action.Section.Name,
                action.Section.SortingIndex,
                true,
                new List<ShoppingListItem> { action.Item });
            sections.Add(section);
        }
        else
        {
            var index = sections.IndexOf(section);
            var items = section.Items.ToList();
            items.Add(action.Item);
            sections[index] = section with { Items = items };
        }

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            },
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                IsSaving = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSetSearchBarActive(ShoppingListState state, SetSearchBarActiveAction action)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                IsActive = true
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnSetSearchBarInactive(ShoppingListState state, SetSearchBarInactiveAction action)
    {
        return state with
        {
            SearchBar = state.SearchBar with
            {
                IsActive = false
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnToggleShoppingListSectionExpansion(ShoppingListState state,
        ToggleShoppingListSectionExpansionAction action)
    {
        var sections = new List<ShoppingListSection>(state.ShoppingList!.Sections);
        var section = sections.FirstOrDefault(s => s.Id == action.SectionId);
        if (section == null)
            return state;

        var sectionIndex = sections.IndexOf(section);

        sections[sectionIndex] = section with { IsExpanded = !section.IsExpanded };

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnRemoveItemFromBasket(ShoppingListState state,
        RemoveItemFromBasketAction action)
    {
        var sections = new List<ShoppingListSection>(state.ShoppingList!.Sections);
        sections = SetBasketStatus(action.ItemId, action.ItemTypeId, false, sections).ToList();

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnPutItemInBasket(ShoppingListState state,
        PutItemInBasketAction action)
    {
        var sections = new List<ShoppingListSection>(state.ShoppingList!.Sections);
        sections = SetBasketStatus(action.ItemId, action.ItemTypeId, true, sections).ToList();

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnChangeItemQuantityFinished(ShoppingListState state,
        ChangeItemQuantityFinishedAction action)
    {
        var sections = new List<ShoppingListSection>(state.ShoppingList!.Sections);
        for (int i = 0; i < sections.Count; i++)
        {
            var section = sections[i];
            var items = section.Items.ToList();
            for (int ii = 0; ii < items.Count; ii++)
            {
                var item = items[ii];
                if (item.Id.ActualId == action.ItemId.ActualId && item.Id.OfflineId == action.ItemId.OfflineId
                                                               && item.TypeId == action.ItemTypeId)
                {
                    items[ii] = item with { Quantity = action.NewQuantity };
                }
            }

            sections[i] = section with { Items = items };
        }

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            }
        };
    }

    [ReducerMethod(typeof(HideItemsInBasketAction))]
    public static ShoppingListState OnHideItemsInBasket(ShoppingListState state)
    {
        var sections = new List<ShoppingListSection>(state.ShoppingList!.Sections);
        for (int i = 0; i < sections.Count; i++)
        {
            var section = sections[i];
            var items = section.Items.Select(item => item with { Hidden = item.IsInBasket }).ToList();
            sections[i] = section with { Items = items };
        }

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnRemoveItemFromShoppingList(ShoppingListState state,
        RemoveItemFromShoppingListAction action)
    {
        var sections = new List<ShoppingListSection>(state.ShoppingList!.Sections);
        for (int i = 0; i < sections.Count; i++)
        {
            var section = sections[i];
            var items = section.Items.ToList();

            var itemToRemove = items
                .FirstOrDefault(item =>
                    item.Id.ActualId == action.ItemId.ActualId && item.Id.OfflineId == action.ItemId.OfflineId
                                                               && item.TypeId == action.ItemTypeId);
            if (itemToRemove is null)
                continue;

            items.Remove(itemToRemove);

            sections[i] = section with { Items = items };
            break;
        }

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            }
        };
    }

    private static IEnumerable<ShoppingListSection> SetBasketStatus(OldModels.ShoppingListItemId itemId,
        Guid? itemTypeId, bool inBasket, IEnumerable<ShoppingListSection> sections)
    {
        var sectionsList = sections.ToList();
        for (int i = 0; i < sectionsList.Count; i++)
        {
            var section = sectionsList[i];
            var items = section.Items.ToList();
            for (int ii = 0; ii < items.Count; ii++)
            {
                var item = items[ii];
                if ((item.Id.ActualId == itemId.ActualId && item.Id.OfflineId == itemId.OfflineId)
                    && item.TypeId == itemTypeId)
                {
                    items[ii] = item with
                    {
                        IsInBasket = inBasket,
                        Hidden = false
                    };
                    sectionsList[i] = section with { Items = items };
                    return sectionsList;
                }
            }
        }

        return sectionsList;
    }
}