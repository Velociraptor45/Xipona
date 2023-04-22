using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;

public class AddItemToShoppingListServiceMock : Mock<IAddItemToShoppingListService>
{
    public AddItemToShoppingListServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupAddItemToShoppingList(IShoppingList shoppingList,
        ItemId itemId, SectionId? sectionId, QuantityInBasket quantity)
    {
        Setup(m => m.AddItemToShoppingListAsync(
                shoppingList,
                itemId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    public void SetupAddItemToShoppingList(IShoppingList shoppingList,
        TemporaryItemId temporaryItemId, SectionId? sectionId, QuantityInBasket quantity)
    {
        Setup(m => m.AddItemToShoppingListAsync(
                shoppingList,
                temporaryItemId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    public void SetupAddItemWithTypeToShoppingList(IShoppingList shoppingList, IItem item,
        ItemTypeId typeId, SectionId? sectionId, QuantityInBasket quantity)
    {
        Setup(m => m.AddItemWithTypeToShoppingList(
                shoppingList,
                item,
                typeId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    public void SetupAddAsync(IEnumerable<ItemToShoppingListAddition> itemsToAdd)
    {
        Setup(m => m.AddAsync(
                itemsToAdd,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    public void VerifyAddItemWithTypeToShoppingList(IShoppingList shoppingList, IItem item,
        ItemTypeId typeId, SectionId? sectionId, QuantityInBasket quantity, Func<Times> times)
    {
        Verify(m => m.AddItemWithTypeToShoppingList(
                shoppingList,
                item,
                typeId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()),
            times);
    }

    public void VerifyAddItemToShoppingListOnce(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        Verify(i => i.AddItemToShoppingListAsync(
                shoppingList,
                itemId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyAddItemToShoppingListOnce(IShoppingList shoppingList, TemporaryItemId temporaryItemId,
        SectionId? sectionId, QuantityInBasket quantity)
    {
        Verify(i => i.AddItemToShoppingListAsync(
                shoppingList,
                temporaryItemId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    public void VerifyAddAsync(IEnumerable<ItemToShoppingListAddition> itemsToAdd, Func<Times> times)
    {
        Verify(m => m.AddAsync(
                itemsToAdd,
                It.IsAny<CancellationToken>()),
            times);
    }
}