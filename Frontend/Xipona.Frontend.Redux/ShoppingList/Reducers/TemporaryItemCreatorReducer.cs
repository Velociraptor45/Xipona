﻿using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;

public static class TemporaryItemCreatorReducer
{
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
    public static ShoppingListState OnTemporaryItemSelectedQuantityTypeChanged(ShoppingListState state,
        TemporaryItemSelectedQuantityTypeChangedAction action)
    {
        return state with
        {
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                SelectedQuantityTypeId = action.QuantityTypeId
            }
        };
    }

    [ReducerMethod(typeof(OpenTemporaryItemCreatorAction))]
    public static ShoppingListState OnOpenTemporaryItemCreator(ShoppingListState state)
    {
        if (state.SelectedStore is null)
            return state;

        var defaultSection = state.SelectedStore.Sections.FirstOrDefault(s => s.IsDefaultSection);

        return state with
        {
            TemporaryItemCreator = state.TemporaryItemCreator with
            {
                ItemName = state.SearchBar.Input,
                IsOpen = true,
                IsSaving = false,
                Price = ShoppingListFeatureState.InitialTemporaryItemPrice,
                Section = defaultSection ?? state.SelectedStore.Sections.First(),
                SelectedQuantityTypeId = state.QuantityTypes.FirstOrDefault()?.Id ?? 0
            }
        };
    }

    [ReducerMethod(typeof(CloseTemporaryItemCreatorAction))]
    public static ShoppingListState OnCloseTemporaryItemCreator(ShoppingListState state)
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

    [ReducerMethod(typeof(SaveTemporaryItemStartedAction))]
    public static ShoppingListState OnSaveTemporaryItemStarted(ShoppingListState state)
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
    public static ShoppingListState OnAddTemporaryItem(ShoppingListState state,
        AddTemporaryItemAction action)
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
    public static ShoppingListState OnTemporaryItemCreated(ShoppingListState state, TemporaryItemCreatedAction action)
    {
        var sections = state.ShoppingList!.Sections.ToList();

        if (!TryFindItem(sections, action.ItemTempId, out var indexes))
        {
            return state;
        }

        var (sectionIndex, itemIndex) = indexes;

        var existingSection = sections[sectionIndex];
        var existingItem = existingSection.Items.ElementAt(itemIndex);
        var items = existingSection.Items.ToList();
        items[itemIndex] = existingItem with
        {
            Id = new(existingItem.Id.OfflineId, action.ItemId)
        };

        var newSection = existingSection with
        {
            Items = items
        };

        sections[sectionIndex] = newSection;

        return state with
        {
            ShoppingList = state.ShoppingList with
            {
                Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
            }
        };
    }

    private static bool TryFindItem(IList<ShoppingListSection> sections, Guid itemTempId,
        out (int SectionIndex, int ItemIndex) indexes)
    {
        for (var i = 0; i < sections.Count; i++)
        {
            var section = sections[i];

            for (var j = 0; j < section.Items.Count; j++)
            {
                var item = section.Items.ElementAt(j);
                if (item.Id.OfflineId == itemTempId)
                {
                    indexes = (i, j);
                    return true;
                }
            }
        }

        indexes = (0, 0);
        return false;
    }
}