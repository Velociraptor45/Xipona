using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Exchanges;

public class ShoppingListExchangeServiceMock : Mock<IShoppingListExchangeService>
{
    public ShoppingListExchangeServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupExchangeItemAsync(ItemId oldItemId, IItem newItem)
    {
        Setup(m => m.ExchangeItemAsync(oldItemId, newItem, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    public void VerifyExchangeItemAsync(ItemId oldItemId, IItem newItem, Func<Times> times)
    {
        Verify(m => m.ExchangeItemAsync(oldItemId, newItem, It.IsAny<CancellationToken>()), times);
    }
}