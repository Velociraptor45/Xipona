namespace ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

public static class ShoppingListEntityMother
{
    public static ShoppingListEntityBuilder InitialWithOneItem(Guid itemId, Guid? itemTypeId, Guid sectionId)
    {
        var shoppingListId = Guid.NewGuid();
        var items = GetItem(shoppingListId)
            .WithItemId(itemId)
            .WithItemTypeId(itemTypeId)
            .WithSectionId(sectionId)
            .CreateMany(1)
            .ToArray();

        return new ShoppingListEntityBuilder()
            .WithId(shoppingListId)
            .WithoutCompletionDate()
            .WithItemsOnList(items);
    }

    public static ShoppingListEntityBuilder InitialWithTwoItems(Guid itemId, Guid? itemTypeId, Guid sectionId)
    {
        var shoppingListId = Guid.NewGuid();
        var item1 = GetItem(shoppingListId)
            .WithItemId(itemId)
            .WithItemTypeId(itemTypeId)
            .WithSectionId(sectionId)
            .Create();
        var item2 = GetItem(shoppingListId).Create();

        return new ShoppingListEntityBuilder()
            .WithId(shoppingListId)
            .WithoutCompletionDate()
            .WithItemsOnList([item1, item2]);
    }

    public static ShoppingListEntityBuilder Empty()
    {
        return new ShoppingListEntityBuilder()
            .WithoutCompletionDate()
            .WithEmptyItemsOnList();
    }

    public static ShoppingListEntityBuilder Active()
    {
        var shoppingListId = Guid.NewGuid();
        var items = Enumerable.Range(0, 3).Select(_ => GetItem(shoppingListId).Create()).ToList();

        return new ShoppingListEntityBuilder()
            .WithId(shoppingListId)
            .WithoutCompletionDate()
            .WithItemsOnList(items);
    }

    private static ItemsOnListEntityBuilder GetItem(Guid shoppingListId)
    {
        return new ItemsOnListEntityBuilder()
            .WithShoppingListId(shoppingListId);
    }
}