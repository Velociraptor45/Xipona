using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Comparer
{
    public class SortingIndexComparer : IComparer<ShoppingListSection>
    {
        public int Compare(ShoppingListSection x, ShoppingListSection y)
        {
            return x.SortingIndex.CompareTo(y.SortingIndex);
        }
    }
}