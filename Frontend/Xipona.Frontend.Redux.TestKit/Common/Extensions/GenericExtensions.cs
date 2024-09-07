namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Common.Extensions;

public static class GenericExtensions
{
    private static readonly Random _random = new Random();

    public static (T, int Index) Random<T>(this IEnumerable<T> source)
    {
        var array = source.ToArray();
        var index = _random.Next(array.Length);
        return (array[index], index);
    }
}