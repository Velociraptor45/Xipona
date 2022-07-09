using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;

public static class StoreMother
{
    public static StoreBuilder Initial(StoreBuilder? builder = null)
    {
        if (builder == null)
            builder = new StoreBuilder();
        var sections = new Sections(
            SectionMother.Default().Create().ToMonoList(),
            new SectionFactoryMock(MockBehavior.Strict).Object);

        return builder
            .WithIsDeleted(false)
            .WithSections(sections);
    }

    public static StoreBuilder Empty(StoreBuilder? builder = null)
    {
        if (builder == null)
            builder = new StoreBuilder();

        return builder
            .WithIsDeleted(false)
            .WithoutSections();
    }

    public static StoreBuilder Sections(int count, StoreBuilder? builder = null)
    {
        if (builder == null)
            builder = new StoreBuilder();

        if (count == 0)
            return Empty(builder);
        if (count == 1)
            return Initial(builder);

        var defaultSection = SectionMother.Default().Create();
        var allSections = SectionMother.NotDefault().CreateMany(count - 1).ToList();

        allSections.Add(defaultSection);
        var sections = new Sections(allSections, new SectionFactoryMock(MockBehavior.Strict).Object);
        return builder
            .WithIsDeleted(false)
            .WithSections(sections);
    }

    public static StoreBuilder Deleted(StoreBuilder? builder = null)
    {
        if (builder == null)
            builder = new StoreBuilder();

        return builder
            .WithIsDeleted(true);
    }
}