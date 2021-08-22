namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
{
    public static class ShoppingListMother
    {
        public static ShoppingListBuilder Initial()
        {
            return new ShoppingListBuilder()
                .WithoutCompletionDate()
                .WithoutSections();
        }

        public static ShoppingListBuilder OneSectionWithOneItemInBasket()
        {
            return new ShoppingListBuilder()
                .WithSection(ShoppingListSectionMother.OneItemInBasket().Create())
                .WithoutCompletionDate();
        }

        public static ShoppingListBuilder OneSectionWithOneItemNotInBasket()
        {
            return new ShoppingListBuilder()
                .WithSection(ShoppingListSectionMother.OneItemNotInBasket().Create())
                .WithoutCompletionDate();
        }

        public static ShoppingListBuilder OneEmptySection()
        {
            return new ShoppingListBuilder()
                .WithSection(ShoppingListSectionMother.Empty().Create())
                .WithoutCompletionDate();
        }

        public static ShoppingListBuilder Sections(int count)
        {
            return new ShoppingListBuilder()
                .WithSections(new ShoppingListSectionBuilder().CreateMany(count));
        }

        public static ShoppingListBuilder Completed()
        {
            return new ShoppingListBuilder();
        }
    }
}