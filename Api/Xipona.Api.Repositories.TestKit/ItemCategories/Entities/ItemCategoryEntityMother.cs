namespace ProjectHermes.Xipona.Api.Repositories.TestKit.ItemCategories.Entities;

public static class ItemCategoryEntityMother
{
    public static ItemCategoryEntityBuilder Active()
    {
        return new ItemCategoryEntityBuilder()
            .WithDeleted(false);
    }
}