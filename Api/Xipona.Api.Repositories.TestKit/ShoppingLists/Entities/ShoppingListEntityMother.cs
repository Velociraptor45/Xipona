namespace ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

public static class ShoppingListEntityMother
{
    public static ShoppingListEntityBuilder InitialWithOneItem()
    {
        return InitialWithOneItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
    }

    public static ShoppingListEntityBuilder InitialWithOneItem(Guid itemId, Guid? itemTypeId, Guid sectionId)
    {
        var shoppingListId = Guid.NewGuid();
        var items = ItemsOnListEntityMother.WithItemTypeId(shoppingListId)
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
        var item1 = ItemsOnListEntityMother.WithItemTypeId(shoppingListId)
            .WithItemId(itemId)
            .WithItemTypeId(itemTypeId)
            .WithSectionId(sectionId)
            .Create();
        var item2 = ItemsOnListEntityMother.WithItemTypeId(shoppingListId).Create();

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
        var items = ItemsOnListEntityMother.WithItemTypeId(shoppingListId).CreateMany(3).ToList();

        return new ShoppingListEntityBuilder()
            .WithId(shoppingListId)
            .WithoutCompletionDate()
            .WithItemsOnList(items);
    }

    public static ShoppingListEntityBuilder ActiveWithItemsWithoutType()
    {
        var shoppingListId = Guid.NewGuid();
        var items = ItemsOnListEntityMother.WithoutItemTypeId(shoppingListId).CreateMany(3).ToList();

        return new ShoppingListEntityBuilder()
            .WithId(shoppingListId)
            .WithoutCompletionDate()
            .WithItemsOnList(items);
    }
}