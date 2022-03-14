using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.AutoFixture.Selectors;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;

namespace ShoppingList.Api.Domain.TestKit.Shared;

public class CommonFixture
{
    private static readonly Random _random = new();

    public Fixture GetNewFixture()
    {
        var fixture = new Fixture();
        fixture.Customizations.Add(new EnumSpecimenBuilder<QuantityType>());
        fixture.Customizations.Add(new TypeRelay(typeof(IStoreItemAvailability), typeof(StoreItemAvailability)));
        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        fixture.Customize<ItemCategoryId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ManufacturerId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ShoppingListId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ItemId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ItemTypeId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<TemporaryItemId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<StoreId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<SectionId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        return fixture;
    }

    public TEnum ChooseRandom<TEnum>() where TEnum : Enum
    {
        IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        return ChooseRandom(values);
    }

    public T ChooseRandom<T>(IEnumerable<T> enumerable)
    {
        if (!enumerable.Any())
            throw new ArgumentException($"{nameof(enumerable)} must at least contain one element.");

        List<T> list = enumerable.ToList();

        int index = NextInt(0, list.Count - 1);
        return list[index];
    }

    public IEnumerable<int> NextUniqueInts(int amount, IEnumerable<int> exclude = null)
    {
        if (amount < 0)
            throw new ArgumentException($"{nameof(amount)} mustn't be negative.");

        List<int> numbers = new List<int>();
        List<int> excludedNumbers = exclude?.ToList() ?? new List<int>();

        for (int i = 0; i < amount; i++)
        {
            int number;
            do
            {
                number = NextInt();
            } while (numbers.Contains(number) || excludedNumbers.Contains(number));
            numbers.Add(number);
        }
        return numbers;
    }

    public int NextInt(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }

    public int NextInt()
    {
        return NextInt(1, int.MaxValue);
    }

    public int NextInt(IEnumerable<int> exclude)
    {
        List<int> excludedInts = exclude.ToList();
        while (true)
        {
            var number = NextInt();
            if (!excludedInts.Contains(number))
                return number;
        }
    }

    public int NextInt(int exclude)
    {
        return NextInt(new List<int> { exclude });
    }

    public bool NextBool()
    {
        return _random.NextDouble() < .5f;
    }

    public float NextFloat()
    {
        return (float)_random.NextDouble();
    }

    public DateTime NextDate()
    {
        var fixture = GetNewFixture();
        return fixture.Create<DateTime>();
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