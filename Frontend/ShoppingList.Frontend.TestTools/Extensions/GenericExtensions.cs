namespace ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

public static class GenericExtensions
{
    public static bool IsEquivalentTo<T>(this T left, T right)
    {
        var comparer = new ObjectsComparer.Comparer<T>();
        return comparer.Compare(left, right);
    }
}