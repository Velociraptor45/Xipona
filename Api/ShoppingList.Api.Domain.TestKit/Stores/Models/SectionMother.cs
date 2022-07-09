namespace ShoppingList.Api.Domain.TestKit.Stores.Models;

public static class SectionMother
{
    public static SectionBuilder Default()
    {
        return new SectionBuilder()
            .WithIsDefaultSection(true);
    }

    public static SectionBuilder NotDefault()
    {
        return new SectionBuilder()
            .WithIsDefaultSection(false);
    }
}