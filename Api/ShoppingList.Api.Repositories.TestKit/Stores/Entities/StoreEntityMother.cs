using ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.Stores.Entities;

public static class StoreEntityMother
{
    public static StoreEntityBuilder Initial()
    {
        var sections = new List<Section>()
        {
            new SectionEntityBuilder()
                .WithIsDefaultSection(false)
                .WithSortIndex(0)
                .WithIsDeleted(false)
                .Create(),
            new SectionEntityBuilder()
                .WithIsDefaultSection(true)
                .WithSortIndex(1)
                .WithIsDeleted(false)
                .Create(),
            new SectionEntityBuilder()
                .WithIsDefaultSection(false)
                .WithSortIndex(2)
                .WithIsDeleted(false)
                .Create()
        };
        return new StoreEntityBuilder()
            .WithDeleted(false)
            .WithSections(sections);
    }

    public static StoreEntityBuilder ValidSections(IEnumerable<Guid> sectionIds)
    {
        var sectionIdArray = sectionIds.ToArray();

        var sections = new List<Section>();

        if (!sectionIdArray.Any())
            return new StoreEntityBuilder().WithDeleted(false).WithSections(sections);

        sections.Add(new SectionEntityBuilder()
            .WithId(sectionIdArray.First())
            .WithIsDefaultSection(true)
            .WithIsDeleted(false)
            .Create());

        for (int i = 1; i < sectionIdArray.Length; i++)
        {
            sections.Add(new SectionEntityBuilder()
                .WithId(sectionIdArray[i])
                .WithIsDefaultSection(false)
                .WithIsDeleted(false)
                .Create());
        }

        return new StoreEntityBuilder().WithDeleted(false).WithSections(sections);
    }

    public static StoreEntityBuilder ActiveAndDeletedSection()
    {
        var sections = new List<Section>()
        {
            new SectionEntityBuilder()
                .WithIsDefaultSection(false)
                .WithSortIndex(0)
                .WithIsDeleted(false)
                .Create(),
            new SectionEntityBuilder()
                .WithIsDefaultSection(true)
                .WithSortIndex(1)
                .WithIsDeleted(true)
                .Create(),
            new SectionEntityBuilder()
                .WithIsDefaultSection(false)
                .WithSortIndex(2)
                .WithIsDeleted(false)
                .Create()
        };
        return new StoreEntityBuilder()
            .WithDeleted(false)
            .WithSections(sections);
    }

    public static StoreEntityBuilder Deleted()
    {
        return Initial()
            .WithDeleted(true);
    }
}