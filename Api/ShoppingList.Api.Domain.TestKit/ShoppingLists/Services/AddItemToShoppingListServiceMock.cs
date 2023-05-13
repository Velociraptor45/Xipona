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

    public void SetupAddItemAsync(IShoppingList shoppingList,
        ItemId itemId, SectionId? sectionId, QuantityInBasket quantity)
    {
        Setup(m => m.AddItemAsync(
                shoppingList,
                itemId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    public void SetupAddItemWithTypeAsync(IShoppingList shoppingList, IItem item,
        ItemTypeId typeId, SectionId? sectionId, QuantityInBasket quantity)
    {
        Setup(m => m.AddItemWithTypeAsync(
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

    public void VerifyAddItemWithTypeAsync(IShoppingList shoppingList, IItem item,
        ItemTypeId typeId, SectionId? sectionId, QuantityInBasket quantity, Func<Times> times)
    {
        Verify(m => m.AddItemWithTypeAsync(
                shoppingList,
                item,
                typeId,
                sectionId,
                quantity,
                It.IsAny<CancellationToken>()),
            times);
    }

    public void VerifyAddItemAsyncOnce(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        Verify(i => i.AddItemAsync(
                shoppingList,
                itemId,
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