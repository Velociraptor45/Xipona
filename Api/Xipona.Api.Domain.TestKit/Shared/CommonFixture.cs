namespace ProjectHermes.Xipona.Api.Domain.TestKit.Shared;

public class CommonFixture
{
    private static readonly Random _random = new();

    public T ChooseRandom<T>(IEnumerable<T> enumerable)
    {
        if (!enumerable.Any())
            throw new ArgumentException($"{nameof(enumerable)} must at least contain one element.");

        List<T> list = enumerable.ToList();

        int index = NextInt(0, list.Count - 1);
        return list[index];
    }

    public int NextInt(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }

    public IEnumerable<T> RemoveRandom<T>(IEnumerable<T> enumerable, int count)
    {
        var list = enumerable.ToList();
        for (int i = 0; i < count; i++)
        {
            var index = NextInt(0, list.Count - 1);
            list.RemoveAt(index);
        }

        return list;
    }

    public IEnumerable<IEnumerable<T>> SplitRandom<T>(IEnumerable<T> enumerable, int targetListCount)
    {
        var list = enumerable.ToList();
        var lists = new List<List<T>>();

        for (int i = 0; i < targetListCount; i++)
        {
            lists.Add(new List<T>());
        }

        while (list.Any())
        {
            var item = list[0];
            list.RemoveAt(0);

            var targetListIndex = NextInt(0, targetListCount - 1);
            lists[targetListIndex].Add(item);
        }

        return lists;
    }
}