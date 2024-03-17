namespace ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

public static class ShoppingListEntityMother
{
    public static ShoppingListEntityBuilder InitialWithOneItem(Guid itemId, Guid? itemTypeId, Guid sectionId)
    {
        var items = new ItemsOnListEntityBuilder()
            .WithItemId(itemId)
            .WithItemTypeId(itemTypeId)
            .WithSectionId(sectionId)
            .WithoutShoppingList()
            .CreateMany(1)
            .ToArray();

        return new ShoppingListEntityBuilder()
            .WithoutCompletionDate()
            .WithItemsOnList(items);
    }

    public static ShoppingListEntityBuilder InitialWithTwoItems(Guid itemId, Guid? itemTypeId, Guid sectionId)
    {
        var item1 = new ItemsOnListEntityBuilder()
            .WithItemId(itemId)
            .WithItemTypeId(itemTypeId)
            .WithSectionId(sectionId)
            .WithoutShoppingList()
            .Create();
        var item2 = new ItemsOnListEntityBuilder()
            .WithoutShoppingList()
            .Create();

        return new ShoppingListEntityBuilder()
            .WithoutCompletionDate()
            .WithItemsOnList(new[] { item1, item2 });
    }

    public static ShoppingListEntityBuilder Empty()
    {
        return new ShoppingListEntityBuilder()
            .WithoutCompletionDate()
            .WithEmptyItemsOnList();
    }
}