using Fluxor;
using Xipona.Frontend.Redux.Items.Actions.Merges;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Items.States.Merges;
using Xipona.Frontend.Redux.Shared.States.Validators;

namespace Xipona.Frontend.Redux.Items.Reducers;

public static class ItemMergeReducer
{
    private static readonly NameValidator _nameValidator = new();

    [ReducerMethod]
    public static ItemState OnInitializeMergingFinished(ItemState state, InitializeMergingFinishedAction action)
    {
        var prefix = string.Empty;
        if (action.Items.Count > 1)
        {
            // get shared prefix
            var firstName = action.Items.First().Name;
            var prefixLength = firstName.Length;

            foreach (var name in action.Items.Skip(1).Select(i => i.Name))
            {
                prefixLength = Math.Min(prefixLength, name.Length);

                for (var i = 0; i < prefixLength; i++)
                {
                    if (firstName[i] != name[i])
                    {
                        prefixLength = i;
                        break;
                    }
                }

                if (prefixLength == 0)
                    break;
            }

            prefix = firstName[..prefixLength].Trim();
        }

        return state with
        {
            Merge = state.Merge with
            {
                Item = new MergedItem(
                    prefix,
                    action.Items.Select(i => new MergedItemType(Guid.NewGuid(), i, i.Name.Remove(0, prefix.Length).Trim())).ToList()),
                IsSaving = false
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnItemTypeNameChanged(ItemState state, ItemTypeNameChangedAction action)
    {
        if (state.Merge.Item is null)
            return state;

        var types = state.Merge.Item.Types.ToList();
        var typeIndex = types.FindIndex(t => t.Key == action.MergedItemTypeKey);
        if (typeIndex < 0)
            return state;

        types[typeIndex] = types[typeIndex] with { Name = action.NewName };

        var typeNameErrors = state.Merge.ValidationResult.TypeNames.ToDictionary();
        if (_nameValidator.Validate(action.NewName, out var typeNameError))
            typeNameErrors.Remove(action.MergedItemTypeKey);
        else
            typeNameErrors[action.MergedItemTypeKey] = typeNameError;

        return state with
        {
            Merge = state.Merge with
            {
                Item = state.Merge.Item with
                {
                    Types = types
                },
                ValidationResult = state.Merge.ValidationResult with
                {
                    TypeNames = typeNameErrors
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnItemNameChanged(ItemState state, ItemNameChangedAction action)
    {
        if (state.Merge.Item is null)
            return state;

        _nameValidator.Validate(action.NewName, out var nameError);

        return state with
        {
            Merge = state.Merge with
            {
                Item = state.Merge.Item with
                {
                    Name = action.NewName
                },
                ValidationResult = state.Merge.ValidationResult with
                {
                    Name = nameError
                }
            }
        };
    }

    [ReducerMethod(typeof(OpenMergeItemSelectorAction))]
    public static ItemState OnOpenMergeItemSelector(ItemState state)
    {
        return state with
        {
            Merge = state.Merge with
            {
                Selector = state.Merge.Selector with
                {
                    SearchResults = [],
                    SelectedItems = [],
                    IsOpen = true,
                    IsSearching = false,
                }
            }
        };
    }

    [ReducerMethod(typeof(CloseMergeItemSelectorAction))]
    public static ItemState OnCloseMergeItemSelector(ItemState state)
    {
        return state with
        {
            Merge = state.Merge with
            {
                Selector = state.Merge.Selector with
                {
                    IsOpen = false
                }
            }
        };
    }

    [ReducerMethod(typeof(SearchItemsForMergeStartedAction))]
    public static ItemState OnSearchItemsForMergeStarted(ItemState state)
    {
        return state with
        {
            Merge = state.Merge with
            {
                Selector = state.Merge.Selector with
                {
                    IsSearching = true
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnSearchItemsForMergeFinished(ItemState state, SearchItemsForMergeFinishedAction action)
    {
        return state with
        {
            Merge = state.Merge with
            {
                Selector = state.Merge.Selector with
                {
                    SearchResults = action.SearchResults,
                    IsSearching = false
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnSelectedMergeItemsChanged(ItemState state, SelectedMergeItemsChangedAction action)
    {
        return state with
        {
            Merge = state.Merge with
            {
                Selector = state.Merge.Selector with
                {
                    SelectedItems = action.Items
                }
            }
        };
    }

    [ReducerMethod(typeof(InitializeMergingAction))]
    public static ItemState OnInitializeMerging(ItemState state)
    {
        return state with
        {
            Merge = state.Merge with
            {
                Selector = state.Merge.Selector with
                {
                    SelectedItems = [],
                    SearchResults = [],
                },
                ValidationResult = new()
            }
        };
    }

    [ReducerMethod(typeof(EnterMergerAction))]
    public static ItemState OnEnterMergerAction(ItemState state)
    {
        return state with
        {
            Merge = state.Merge with
            {
                Selector = state.Merge.Selector with
                {
                    IsOpen = false,
                    IsSearching = false,
                }
            }
        };
    }

    [ReducerMethod(typeof(MergeItemsStartedAction))]
    public static ItemState OnMergeItemsStarted(ItemState state)
    {
        return state with
        {
            Merge = state.Merge with
            {
                IsSaving = true
            }
        };
    }

    [ReducerMethod(typeof(MergeItemsFinishedAction))]
    public static ItemState OnMergeItemsFinished(ItemState state)
    {
        return state with
        {
            Merge = state.Merge with
            {
                IsSaving = false
            }
        };
    }

    [ReducerMethod(typeof(MergeItemsAction))]
    public static ItemState OnMergeItemsAction(ItemState state)
    {
        if (state.Merge.Item is null)
            return state;

        _nameValidator.Validate(state.Merge.Item.Name, out var typeNameError);

        Dictionary<Guid, string> typeNameErrors = new();
        foreach (var type in state.Merge.Item.Types)
        {
            if (!_nameValidator.Validate(type.Name, out var error))
                typeNameErrors[type.Key] = error!;
        }

        return state with
        {
            Merge = state.Merge with
            {
                ValidationResult = new ItemMergeValidationResult(typeNameError, typeNameErrors)
            }
        };
    }
}
