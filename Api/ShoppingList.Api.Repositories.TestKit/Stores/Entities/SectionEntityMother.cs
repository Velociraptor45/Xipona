namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.Stores.Entities;

public static class SectionEntityMother
{
    public static SectionEntityBuilder Default()
    {
        return new SectionEntityBuilder()
            .WithIsDefaultSection(true)
            .WithIsDeleted(false);
    }
}