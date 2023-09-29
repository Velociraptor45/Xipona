using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions.Editor.Sections;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.Reducers;

public static class StoreEditorReducer
{
    [ReducerMethod(typeof(SetNewStoreAction))]
    public static StoreState OnSetNewStore(StoreState state)
    {
        var sections = new List<EditedSection>
        {
            new(Guid.NewGuid(), Guid.Empty, "Default", true, 0)
        };

        return state with
        {
            Editor = state.Editor with
            {
                Store = new EditedStore(
                    Guid.Empty,
                    string.Empty,
                    new SortedSet<EditedSection>(sections, new SortingIndexComparer()))
            }
        };
    }

    [ReducerMethod]
    public static StoreState OnLoadStoreForEditingFinished(StoreState state, LoadStoreForEditingFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                Store = action.Store
            }
        };
    }

    [ReducerMethod]
    public static StoreState OnStoreNameChanged(StoreState state, StoreNameChangedAction action)
    {
        if (state.Editor.Store is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Store = state.Editor.Store with
                {
                    Name = action.Name
                }
            }
        };
    }

    [ReducerMethod(typeof(SaveStoreStartedAction))]
    public static StoreState OnSaveStoreStarted(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod(typeof(SaveStoreFinishedAction))]
    public static StoreState OnSaveStoreFinished(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsSaving = false
            }
        };
    }

    [ReducerMethod]
    public static StoreState OnSectionDecremented(StoreState state, SectionDecrementedAction action)
    {
        if (state.Editor.Store is null)
            return state;

        var sections = state.Editor.Store.Sections.ToList();
        var section = sections.FirstOrDefault(s => s.Key == action.SectionKey);
        if (section is null)
            return state;

        int sectionIndex = sections.IndexOf(section);
        if (sectionIndex is -1 or <= 0)
            return state;

        var decremented = sections[sectionIndex];
        var incremented = sections[sectionIndex - 1];

        sections[sectionIndex - 1] = incremented with { SortingIndex = decremented.SortingIndex };
        sections[sectionIndex] = decremented with { SortingIndex = incremented.SortingIndex };

        return state with
        {
            Editor = state.Editor with
            {
                Store = state.Editor.Store with
                {
                    Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod]
    public static StoreState OnSectionIncremented(StoreState state, SectionIncrementedAction action)
    {
        if (state.Editor.Store is null)
            return state;

        var sections = state.Editor.Store.Sections.ToList();
        var section = sections.FirstOrDefault(s => s.Key == action.SectionKey);
        if (section is null)
            return state;

        int sectionIndex = sections.IndexOf(section);
        if (sectionIndex == -1 || sectionIndex >= sections.Count - 1)
            return state;

        var incremented = sections[sectionIndex];
        var decremented = sections[sectionIndex + 1];

        sections[sectionIndex + 1] = decremented with { SortingIndex = incremented.SortingIndex };
        sections[sectionIndex] = incremented with { SortingIndex = decremented.SortingIndex };

        return state with
        {
            Editor = state.Editor with
            {
                Store = state.Editor.Store with
                {
                    Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod]
    public static StoreState OnSectionRemoved(StoreState state, SectionRemovedAction action)
    {
        if (state.Editor.Store is null)
            return state;

        var sections = state.Editor.Store.Sections.ToList();
        sections.Remove(action.Section);

        return state with
        {
            Editor = state.Editor with
            {
                Store = state.Editor.Store with
                {
                    Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod]
    public static StoreState OnSectionTextChanged(StoreState state, SectionTextChangedAction action)
    {
        if (state.Editor.Store is null)
            return state;

        var sections = state.Editor.Store.Sections.ToList();
        var step = sections.FirstOrDefault(s => s.Key == action.SectionKey);
        if (step is null)
            return state;

        var stepIndex = sections.IndexOf(step);
        sections[stepIndex] = step with { Name = action.Text };

        return state with
        {
            Editor = state.Editor with
            {
                Store = state.Editor.Store with
                {
                    Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod]
    public static StoreState OnDefaultSectionChanged(StoreState state, DefaultSectionChangedAction action)
    {
        if (state.Editor.Store is null)
            return state;

        if (state.Editor.Store.Sections.All(s => s.Key != action.SectionKey))
            return state;

        var sections = state.Editor.Store!.Sections
            .Select(s => s with { IsDefaultSection = s.Key == action.SectionKey })
            .ToList();

        return state with
        {
            Editor = state.Editor with
            {
                Store = state.Editor.Store with
                {
                    Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod(typeof(SectionAddedAction))]
    public static StoreState OnSectionAdded(StoreState state)
    {
        if (state.Editor.Store is null)
            return state;

        var sections = state.Editor.Store.Sections.ToList();
        var nextSortingIndex = state.Editor.Store.Sections.Max?.SortingIndex + 1 ?? 0;
        sections.Add(new EditedSection(Guid.NewGuid(), Guid.Empty, string.Empty, false, nextSortingIndex));

        return state with
        {
            Editor = state.Editor with
            {
                Store = state.Editor.Store with
                {
                    Sections = new SortedSet<EditedSection>(sections, new SortingIndexComparer())
                }
            }
        };
    }

    [ReducerMethod(typeof(DeleteStoreButtonClickedAction))]
    public static StoreState OnDeleteStoreButtonClicked(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsShowingDeletionNotice = true
            }
        };
    }

    [ReducerMethod(typeof(DeleteStoreAbortedAction))]
    public static StoreState OnDeleteStoreAborted(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsShowingDeletionNotice = false
            }
        };
    }

    [ReducerMethod(typeof(DeleteStoreStartedAction))]
    public static StoreState OnDeleteStoreStarted(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleting = true
            }
        };
    }

    [ReducerMethod(typeof(DeleteStoreFinishedAction))]
    public static StoreState OnDeleteStoreFinished(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsDeleting = false
            }
        };
    }

    [ReducerMethod(typeof(CloseDeleteStoreDialogAction))]
    public static StoreState OnCloseDeleteStoreDialog(StoreState state)
    {
        return state with
        {
            Editor = state.Editor with
            {
                IsShowingDeletionNotice = false
            }
        };
    }
}