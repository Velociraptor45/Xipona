namespace ProjectHermes.ShoppingList.Frontend.Models.TestKit.Stores.Models;

public static class SectionMother
{
    public static SectionBuilder Default()
    {
        return new SectionBuilder().WithIsDefaultSection(true);
    }

    public static SectionBuilder NotDefault()
    {
        return new SectionBuilder().WithIsDefaultSection(false);
    }
}