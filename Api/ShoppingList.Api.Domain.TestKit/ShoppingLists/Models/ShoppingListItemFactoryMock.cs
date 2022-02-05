using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListItemFactoryMock : Mock<IShoppingListItemFactory>
{
    public ShoppingListItemFactoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupCreate(ItemId itemId, ItemTypeId? itemTypeId, bool isInBasket, float quantity,
        IShoppingListItem returnValue)
    {
        Setup(instance => instance.Create(
                itemId,
                itemTypeId,
                isInBasket,
                quantity))
            .Returns(returnValue);
    }

    public void VerifyCreateOnce(ItemId itemId, ItemTypeId? itemTypeId, bool isInBasket, float quantity)
    {
        Verify(i => i.Create(
                itemId,
                itemTypeId,
                isInBasket,
                quantity),
            Times.Once);
    }
}