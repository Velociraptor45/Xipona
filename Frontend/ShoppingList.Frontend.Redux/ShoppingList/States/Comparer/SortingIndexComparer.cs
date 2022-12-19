namespace ShoppingList.Frontend.Redux.ShoppingList.States.Comparer
{
    public class SortingIndexComparer : IComparer<ShoppingListSection>
    {
        public int Compare(ShoppingListSection? x, ShoppingListSection? y)
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
}