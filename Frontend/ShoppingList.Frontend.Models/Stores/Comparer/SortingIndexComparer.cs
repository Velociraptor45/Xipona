using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.Stores.Comparer
{
    public class SortingIndexComparer : IComparer<StoreSection>
    {
        public int Compare(StoreSection x, StoreSection y)
        {
            return x.SortingIndex.CompareTo(y.SortingIndex);
        }
    }
}