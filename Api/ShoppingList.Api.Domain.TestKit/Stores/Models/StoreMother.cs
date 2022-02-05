namespace ShoppingList.Api.Domain.TestKit.Stores.Models;

public static class StoreMother
{
    public static StoreBuilder Initial(StoreBuilder builder = null)
    {
        if (builder == null)
            builder = new StoreBuilder();

        return builder
            .WithIsDeleted(false)
            .WithSection(StoreSectionMother.Default().Create());
    }

    public static StoreBuilder Empty(StoreBuilder builder = null)
    {
        if (builder == null)
            builder = new StoreBuilder();

        return builder
            .WithIsDeleted(false)
            .WithoutSections();
    }

    public static StoreBuilder Sections(int count, StoreBuilder builder = null)
    {
        if (builder == null)
            builder = new StoreBuilder();

        if (count == 0)
            return Empty(builder);
        else if (count == 1)
            return Initial(builder);

        var defaultSection = StoreSectionMother.Default().Create();
        var allSections = StoreSectionMother.NotDefault().CreateMany(count - 1).ToList();

        allSections.Add(defaultSection);
        return builder
            .WithIsDeleted(false)
            .WithSections(allSections);
    }

    public static StoreBuilder Deleted(StoreBuilder builder = null)
    {
        if (builder == null)
            builder = new StoreBuilder();

        return builder
            .WithIsDeleted(true);
    }
}