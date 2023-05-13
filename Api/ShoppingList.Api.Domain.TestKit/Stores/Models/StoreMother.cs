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
        var sections = SectionsMother.Valid(1, new SectionFactoryMock(MockBehavior.Strict).Object);

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

        var sections = SectionsMother.Valid(count, new SectionFactoryMock(MockBehavior.Strict).Object);
        return builder
            .WithIsDeleted(false)
            .WithSections(sections);
    }

    public static StoreBuilder Section(ISection section)
    {
        return new StoreBuilder()
            .WithIsDeleted(false)
            .WithSections(new Sections(section.ToMonoList(), new SectionFactoryMock(MockBehavior.Strict).Object));
    }

    public static StoreBuilder Deleted(StoreBuilder? builder = null)
    {
        if (builder == null)
            builder = new StoreBuilder();

        return builder
            .WithIsDeleted(true);
    }
}