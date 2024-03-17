namespace ProjectHermes.Xipona.Frontend.TestTools.Extensions;

public static class GenericExtensions
{
    public static bool IsRequestEquivalentTo<T>(this T left, T right)
    {
        return IsRequestEquivalentTo(left, right, new List<string>());
    }

    public static bool IsRequestEquivalentTo<T>(this T left, T right, IEnumerable<string> excludedPaths)
    {
        var excludedPathsList = excludedPaths.ToList();
        excludedPathsList.Add("RequestId");
        return left.IsEquivalentTo(right, excludedPathsList);
    }

    public static bool IsEquivalentTo<T>(this T left, T right)
    {
        var comparer = new ObjectsComparer.Comparer<T>();
        return comparer.Compare(left, right);
    }

    public static bool IsEquivalentTo<T>(this T left, T right, IEnumerable<string> excludedPaths)
    {
        var excludedPathsList = excludedPaths.ToList();

        var comparer = new ObjectsComparer.Comparer<T>();
        var originalDiff = comparer.CalculateDifferences(left, right);
        var filteredDiff = originalDiff
            .Where(x => !excludedPathsList.Contains(x.MemberPath))
            .ToList();
        return !filteredDiff.Any();
    }
}