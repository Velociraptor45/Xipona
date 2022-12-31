namespace ProjectHermes.ShoppingList.Frontend.Models.TestKit.Stores.Models;

public static class ItemStoreSectionMother
{
    public static ItemStoreSectionBuilder Default()
    {
        return new ItemStoreSectionBuilder().WithIsDefaultSection(true);
    }

    public static ItemStoreSectionBuilder NotDefault()
    {
        return new ItemStoreSectionBuilder().WithIsDefaultSection(false);
    }
}