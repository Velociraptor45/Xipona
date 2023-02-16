namespace ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

public static class GenericExtensions
{
    public static bool IsRequestEquivalentTo<T>(this T left, T right)
    {
        var comparer = new ObjectsComparer.Comparer<T>();
        var originalDiff = comparer.CalculateDifferences(left, right);
        var filteredDiff = originalDiff.Where(x => x.MemberPath != "RequestId").ToList();
        return !filteredDiff.Any();
    }

    public static bool IsEquivalentTo<T>(this T left, T right)
    {
        var comparer = new ObjectsComparer.Comparer<T>();
        return comparer.Compare(left, right);
    }
}