using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Items;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

public static class ShoppingListItemReducer
{
    [ReducerMethod]
    public static ShoppingListState OnLoadingPriceUpdaterPricesFinished(ShoppingListState state,
        LoadingPriceUpdaterPricesFinishedAction action)
    {
        var typeId = state.PriceUpdate.Item!.TypeId!.Value;

        var prices = action.Prices.ToList();

        var selected = prices.FirstOrDefault(p => p.ItemTypeId == typeId);
        if (selected is not null)
            prices.Remove(selected);

        return state with
        {
            PriceUpdate = state.PriceUpdate with
            {
                OtherItemTypePrices = prices
            }
        };
    }

    [ReducerMethod]
    public static ShoppingListState OnRemoveItemFromBasket(ShoppingListState state,
        RemoveItemFromBasketAction action)
    {
        var sections = new List<ShoppingListSection>(state.ShoppingList!.Sections);
        sections = SetBasketStatus(action.ItemId, action.ItemTypeId, false, sections);

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
        sections = SetBasketStatus(action.ItemId, action.ItemTypeId, true, sections);

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
                if (item.Id.ActualId == action.ItemId.ActualId
                    && item.Id.OfflineId == action.ItemId.OfflineId
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
                .FirstOrDefault(item => item.Id.ActualId == action.ItemId.ActualId
                                        && item.Id.OfflineId == action.ItemId.OfflineId
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

    private static List<ShoppingListSection> SetBasketStatus(ShoppingListItemId itemId,
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
                if (item.Id.ActualId == itemId.ActualId
                    && item.Id.OfflineId == itemId.OfflineId
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