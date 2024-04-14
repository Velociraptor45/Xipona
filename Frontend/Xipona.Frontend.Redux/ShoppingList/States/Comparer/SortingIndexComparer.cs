using ProjectHermes.Xipona.Frontend.Redux.Shared.States;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;

public class SortingIndexComparer : IComparer<ISortableItem>
{
    public int Compare(ISortableItem? x, ISortableItem? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (ReferenceEquals(null, y))
        {
            return 1;
        }

        if (ReferenceEquals(null, x))
        {
            return -1;
        }

        return x.SortingIndex.CompareTo(y.SortingIndex);
    }
}