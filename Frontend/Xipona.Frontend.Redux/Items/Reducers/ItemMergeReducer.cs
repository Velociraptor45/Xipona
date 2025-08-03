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
        return state with
        {
            Merge = new ItemMerge(
                new MergedItem(
                    string.Empty,
                    action.Items.Select(i => new MergedItemType(Guid.NewGuid(), i, i.Name)).ToList()),
                false)
        };
    }
}
