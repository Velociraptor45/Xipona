namespace ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Models;

public static class ItemCategoryMother
{
    public static ItemCategoryBuilder Deleted()
    {
        return new ItemCategoryBuilder()
            .WithIsDeleted(true);
    }

    public static ItemCategoryBuilder NotDeleted()
    {
        return new ItemCategoryBuilder()
            .WithIsDeleted(false);
    }
}