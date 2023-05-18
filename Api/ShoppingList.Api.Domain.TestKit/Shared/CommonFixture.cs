using AutoFixture.AutoMoq;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;

public class CommonFixture
{
    private static readonly Random _random = new();

    public Fixture GetNewFixture()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        new DomainCustomization().Customize(fixture);
        return fixture;
    }

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
}