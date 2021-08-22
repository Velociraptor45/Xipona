namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
{
    public static class ShoppingListItemMother
    {
        public static ShoppingListItemBuilder InBasket()
        {
            return new ShoppingListItemBuilder()
                .WithIsInBasket(true);
        }

        public static ShoppingListItemBuilder NotInBasket()
        {
            return new ShoppingListItemBuilder()
                .WithIsInBasket(false);
        }
    }
}
