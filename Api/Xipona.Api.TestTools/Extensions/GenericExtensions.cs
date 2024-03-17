namespace ProjectHermes.Xipona.Api.TestTools.Extensions;

public static class GenericExtensions
{
    public static bool IsEquivalentTo<T>(this T left, T right)
    {
        var comparer = new ObjectsComparer.Comparer<T>();
        return comparer.Compare(left, right);
    }

    public static bool IsEquivalentTo<T>(this T left, T right, IEnumerable<string>? excludedProperties = null)
    {
        var comparer = new ObjectsComparer.Comparer<T>();
        var diff = comparer.CalculateDifferences(left, right).ToList();

        if (excludedProperties != null)
            diff.RemoveAll(d => excludedProperties.Contains(d.MemberPath));

        return !diff.Any();
    }
}