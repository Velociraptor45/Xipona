namespace Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

public static class ItemsOnListEntityMother
{
    public static ItemsOnListEntityBuilder ItemInBasket(Guid itemId, Guid shoppingListId)
    {
        return new ItemsOnListEntityBuilder()
            .WithInBasket(true)
            .WithoutItemTypeId()
            .WithItemId(itemId)
            .WithShoppingListId(shoppingListId);
    }

    public static ItemsOnListEntityBuilder ItemType(Guid shoppingListId)
    {
        return new ItemsOnListEntityBuilder()
            .WithShoppingListId(shoppingListId);
    }

    public static ItemsOnListEntityBuilder Item(Guid shoppingListId)
    {
        return new ItemsOnListEntityBuilder()
            .WithShoppingListId(shoppingListId)
            .WithoutItemTypeId();
    }
}