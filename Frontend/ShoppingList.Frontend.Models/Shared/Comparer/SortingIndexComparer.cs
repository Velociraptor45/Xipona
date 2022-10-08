using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Comparer
{
    public class SortingIndexComparer : IComparer<ISortableItem>
    {
        public int Compare(ISortableItem x, ISortableItem y)
        {
            return x.SortingIndex.CompareTo(y.SortingIndex);
        }
    }
}