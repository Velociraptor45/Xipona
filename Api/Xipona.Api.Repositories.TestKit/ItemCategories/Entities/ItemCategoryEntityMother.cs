namespace Xipona.Api.Repositories.TestKit.ItemCategories.Entities;

public static class ItemCategoryEntityMother
{
    public static ItemCategoryEntityBuilder Active()
    {
        return new ItemCategoryEntityBuilder()
            .WithDeleted(false);
    }
}

public class ItemCategoryEntityMother2
{
    private readonly ItemCategoryEntityGodmother _grandmother = new();

    public ItemCategoryEntityMother2()
    {
    }

    public ItemCategoryEntityMother2(ItemCategoryEntityGodmother grandmother)
    {
        _grandmother = grandmother;
    }
    
    public ItemCategoryEntityBuilder Active()
    {
        return _grandmother.GetBasics()
            .WithDeleted(false);
    }
}