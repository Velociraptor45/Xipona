using Fluxor;
using Xipona.Frontend.Redux.Items.Actions.Merges;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Items.States.Merges;

namespace Xipona.Frontend.Redux.Items.Reducers;

public static class ItemMergeReducer
{
    [ReducerMethod]
    public static ItemState OnInitializeMergingFinished(ItemState state, InitializeMergingFinishedAction action)
    {
        var prefix = string.Empty;
        if (action.Items.Count > 1)
        {
            // get shared prefix
            var firstName = action.Items.First().Name;
            var prefixLength = firstName.Length;

            foreach (var item in action.Items.Skip(1))
            {
                prefixLength = Math.Min(prefixLength, item.Name.Length);

                for (var i = 0; i < prefixLength; i++)
                {
                    if (firstName[i] != item.Name[i])
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

        return state with
        {
            Merge = state.Merge with
            {
                Item = state.Merge.Item with
                {
                    Types = types
                }
            }
        };
    }

    [ReducerMethod]
    public static ItemState OnItemNameChanged(ItemState state, ItemNameChangedAction action)
    {
        if (state.Merge.Item is null)
            return state;

        return state with
        {
            Merge = state.Merge with
            {
                Item = state.Merge.Item with
                {
                    Name = action.NewName
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
                    IsOpen = true
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
}
