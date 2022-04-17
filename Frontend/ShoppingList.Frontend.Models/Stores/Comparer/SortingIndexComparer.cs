using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.Stores.Comparer
{
    public class SortingIndexComparer : IComparer<Section>
    {
        public int Compare(Section x, Section y)
        {
            return x.SortingIndex.CompareTo(y.SortingIndex);
        }
    }
}