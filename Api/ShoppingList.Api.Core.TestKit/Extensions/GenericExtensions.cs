using ObjectsComparer;

namespace ShoppingList.Api.Core.TestKit.Extensions
{
    public static class GenericExtensions
    {
        public static bool IsEquivalentTo<T>(this T left, T right)
        {
            var comparer = new Comparer<T>();
            return comparer.Compare(left, right);
        }
    }
}