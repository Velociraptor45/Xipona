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
        return state with { ShoppingList = action.ShoppingList };
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
    public static ShoppingListState OnChangeItemQuantity(ShoppingListState state,
        ChangeItemQuantityAction action)
    {
        var sections = new List<ShoppingListSection>(state.ShoppingList!.Sections);
        foreach (var section in sections)
        {
            var items = section.Items.ToList();
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if ((item.Id.ActualId == action.ItemId.ActualId || item.Id.OfflineId == action.ItemId.OfflineId)
                    && item.TypeId == action.ItemTypeId)
                {
                    items[i] = item with { Quantity = action.Quantity };
                }
            }
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
                    (item.Id.ActualId == action.ItemId.ActualId || item.Id.OfflineId == action.ItemId.OfflineId)
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
                    items[ii] = item with { IsInBasket = inBasket };
                    sectionsList[i] = section with { Items = items };
                    return sectionsList;
                }
            }
        }

        return sectionsList;
    }
}