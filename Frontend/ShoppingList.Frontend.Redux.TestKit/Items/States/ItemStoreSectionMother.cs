namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Items.States;

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