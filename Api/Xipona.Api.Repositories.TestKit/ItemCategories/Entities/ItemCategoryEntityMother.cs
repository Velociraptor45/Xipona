namespace Xipona.Api.Repositories.TestKit.ItemCategories.Entities;

public static class ItemCategoryEntityMother
{
    public static ItemCategoryEntityBuilder Active(ItemCategoryEntityGodmother? godmother = null)
    {
        return (godmother?.GetFoundation() ?? new ItemCategoryEntityBuilder())
            .WithDeleted(false);
    }
}