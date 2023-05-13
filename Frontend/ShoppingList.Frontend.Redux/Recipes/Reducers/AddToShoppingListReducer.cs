using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.AddToShoppingListModal;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;

public static class AddToShoppingListReducer
{
    [ReducerMethod(typeof(AddToShoppingListModalClosedAction))]
    public static RecipeState OnAddToShoppingListModalClosed(RecipeState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsAddToShoppingListOpen = false,
                AddToShoppingList = null
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnLoadAddToShoppingListFinished(RecipeState state,
        LoadAddToShoppingListFinishedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        var amountsForOneServing = action.ItemsForOneServing.ToDictionary(i => (i.ItemId, i.ItemTypeId), i => i.Quantity);

        var items = Multiply(amountsForOneServing, action.ItemsForOneServing, state.Editor.Recipe.NumberOfServings)
            .ToList();

        var addToShoppingList = new AddToShoppingList(
            state.Editor.Recipe.NumberOfServings,
            amountsForOneServing,
            items,
            false);

        return state with
        {
            Editor = state.Editor with
            {
                IsAddToShoppingListOpen = true,
                AddToShoppingList = addToShoppingList
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnAddToShoppingListNumberOfServingsChanged(RecipeState state,
        AddToShoppingListNumberOfServingsChangedAction action)
    {
        if (state.Editor.AddToShoppingList is null)
            return state;

        var items = Multiply(
                state.Editor.AddToShoppingList.ItemAmountsForOneServing,
                state.Editor.AddToShoppingList.Items,
                action.NumberOfServings)
            .ToList();

        return state with
        {
            Editor = state.Editor with
            {
                AddToShoppingList = state.Editor.AddToShoppingList with
                {
                    NumberOfServings = action.NumberOfServings,
                    Items = items
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnAddItemToShoppingListChanged(RecipeState state,
        AddItemToShoppingListChangedAction action)
    {
        if (state.Editor.AddToShoppingList is null)
            return state;

        var items = state.Editor.AddToShoppingList!.Items.ToList();
        var item = items.FirstOrDefault(i => i.Key == action.ItemKey);
        if (item is null)
            return state;

        var itemIndex = items.IndexOf(item);
        items[itemIndex] = item with { AddToShoppingList = action.AddToShoppingList };

        return state with
        {
            Editor = state.Editor with
            {
                AddToShoppingList = state.Editor.AddToShoppingList with
                {
                    Items = items,
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnAddToShoppingListItemStoreChanged(RecipeState state,
        AddToShoppingListItemStoreChangedAction action)
    {
        if (state.Editor.AddToShoppingList is null)
            return state;

        var items = state.Editor.AddToShoppingList!.Items.ToList();
        var item = items.FirstOrDefault(i => i.Key == action.ItemKey);
        if (item is null)
            return state;

        var itemIndex = items.IndexOf(item);
        items[itemIndex] = item with { SelectedStoreId = action.StoreId };

        return state with
        {
            Editor = state.Editor with
            {
                AddToShoppingList = state.Editor.AddToShoppingList with
                {
                    Items = items,
                }
            }
        };
    }

    [ReducerMethod(typeof(AddItemsToShoppingListStartedAction))]
    public static RecipeState OnAddItemsToShoppingListStarted(RecipeState state)
    {
        if (state.Editor.AddToShoppingList is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                AddToShoppingList = state.Editor.AddToShoppingList with
                {
                    IsSaving = true,
                }
            }
        };
    }

    [ReducerMethod(typeof(AddItemsToShoppingListFinishedAction))]
    public static RecipeState OnAddItemsToShoppingListFinished(RecipeState state)
    {
        if (state.Editor.AddToShoppingList is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                AddToShoppingList = state.Editor.AddToShoppingList with
                {
                    IsSaving = false,
                }
            }
        };
    }

    private static IEnumerable<AddToShoppingListItem> Multiply(
        IReadOnlyDictionary<(Guid, Guid?), float> itemAmountsForOneServing,
        IEnumerable<AddToShoppingListItem> items,
        int numberOfServings)
    {
        foreach (var item in items)
        {
            if (!itemAmountsForOneServing.TryGetValue((item.ItemId, item.ItemTypeId), out var quantity))
                continue;

            yield return item with { Quantity = (float)Math.Ceiling(quantity * numberOfServings) };
        }
    }
}