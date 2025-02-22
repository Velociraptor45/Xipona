namespace ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

public static class ItemsOnListEntityMother
{
    public static ItemsOnListEntityBuilder WithItemTypeId(Guid shoppingListId)
    {
        return new ItemsOnListEntityBuilder()
            .WithShoppingListId(shoppingListId);
    }

    public static ItemsOnListEntityBuilder WithoutItemTypeId(Guid shoppingListId)
    {
        return new ItemsOnListEntityBuilder()
            .WithShoppingListId(shoppingListId)
            .WithoutItemTypeId();
    }
}