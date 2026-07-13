namespace Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

public static class ShoppingListEntityMother
{
    public static ShoppingListEntityBuilder InitialWithOneItem()
    {
        return InitialWithOneItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
    }

    public static ShoppingListEntityBuilder InitialWithOneItem(Guid itemId, Guid? itemTypeId, Guid sectionId)
    {
        var shoppingListId = Guid.NewGuid();
        var items = ItemsOnListEntityMother.ItemType(shoppingListId)
            .WithItemId(itemId)
            .WithItemTypeId(itemTypeId)
            .WithSectionId(sectionId)
            .CreateMany(1)
            .ToArray();

        return new ShoppingListEntityBuilder()
            .WithId(shoppingListId)
            .WithoutCompletionDate()
            .WithEmptyDiscounts()
            .WithEmptyListDiscounts()
            .WithItemsOnList(items);
    }

    public static ShoppingListEntityBuilder InitialWithTwoItems(Guid itemId, Guid? itemTypeId, Guid sectionId)
    {
        var shoppingListId = Guid.NewGuid();
        var item1 = ItemsOnListEntityMother.ItemType(shoppingListId)
            .WithItemId(itemId)
            .WithItemTypeId(itemTypeId)
            .WithSectionId(sectionId)
            .Create();
        var item2 = ItemsOnListEntityMother.ItemType(shoppingListId).Create();

        return new ShoppingListEntityBuilder()
            .WithId(shoppingListId)
            .WithoutCompletionDate()
            .WithEmptyDiscounts()
            .WithEmptyListDiscounts()
            .WithItemsOnList([item1, item2]);
    }

    public static ShoppingListEntityBuilder Empty(ShoppingListEntityGodmother? godmother = null)
    {
        return (godmother?.GetFoundation() ?? new ShoppingListEntityBuilder())
            .WithoutCompletionDate()
            .WithEmptyDiscounts()
            .WithEmptyListDiscounts()
            .WithEmptyItemsOnList();
    }

    public static ShoppingListEntityBuilder Active(ShoppingListEntityGodmother? godmother = null)
    {
        return (godmother?.GetFoundation() ?? new ShoppingListEntityBuilder())
            .WithoutCompletionDate();
    }

    public static ShoppingListEntityBuilder ActiveWithoutDiscounts(ShoppingListEntityGodmother? godmother = null)
    {
        return (godmother?.GetFoundation() ?? new ShoppingListEntityBuilder())
            .WithoutCompletionDate()
            .WithEmptyDiscounts()
            .WithEmptyListDiscounts();
    }

    public static ShoppingListEntityBuilder Completed(ShoppingListEntityGodmother? godmother = null)
    {
        return godmother?.GetFoundation() ?? new ShoppingListEntityBuilder();
    }

    public static ShoppingListEntityBuilder ActiveWithItemsWithoutType()
    {
        var shoppingListId = Guid.NewGuid();
        var items = ItemsOnListEntityMother.Item(shoppingListId).CreateMany(3).ToList();

        return new ShoppingListEntityBuilder()
            .WithId(shoppingListId)
            .WithoutCompletionDate()
            .WithEmptyDiscounts()
            .WithEmptyListDiscounts()
            .WithItemsOnList(items);
    }

    public static ShoppingListEntityBuilder ListDiscount()
    {
        return Active()
            .WithListDiscounts([ShoppingListDiscountEntityMother.Price().Create()]);
    }
}