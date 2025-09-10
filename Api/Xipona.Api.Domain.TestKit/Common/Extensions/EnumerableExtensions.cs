namespace Xipona.Api.Domain.TestKit.Common.Extensions;
public static class EnumerableExtensions
{
    private static readonly Random _random = new();

    public static T ChooseRandom<T>(this IEnumerable<T> enumerable)
    {
        return ChooseRandom(enumerable, out var _);
    }

    public static T ChooseRandom<T>(this IEnumerable<T> enumerable, out int index)
    {
        List<T> list = enumerable.ToList();
        if (list.Count == 0)
            throw new ArgumentException($"{nameof(enumerable)} must at least contain one element.");

        index = NextInt(0, list.Count - 1);
        return list[index];
    }

    public static int NextInt(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }

}
