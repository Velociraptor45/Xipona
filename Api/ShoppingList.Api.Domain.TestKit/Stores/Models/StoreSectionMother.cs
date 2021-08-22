namespace ShoppingList.Api.Domain.TestKit.Stores.Models
{
    public static class StoreSectionMother
    {
        public static StoreSectionBuilder Default()
        {
            return new StoreSectionBuilder()
                .WithIsDefaultSection(true);
        }

        public static StoreSectionBuilder NotDefault()
        {
            return new StoreSectionBuilder()
                .WithIsDefaultSection(false);
        }
    }
}