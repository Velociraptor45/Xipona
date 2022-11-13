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
}