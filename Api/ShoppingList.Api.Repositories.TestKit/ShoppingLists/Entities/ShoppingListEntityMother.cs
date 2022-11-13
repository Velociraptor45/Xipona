namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.ShoppingLists.Entities;

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
}